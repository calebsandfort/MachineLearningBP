using MachineLearningBP.CollectiveIntelligence.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Core.Services.Dtos;
using System.Configuration;
using Google.GData.Client;
using MachineLearningBP.Shared.Dtos;
using Google.Apis.Drive.v2;
using System.Reflection;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using Google.Apis.Drive.v2.Data;
using Google.GData.Spreadsheets;
using MachineLearningBP.CollectiveIntelligence.DomainServices.Algorithms.Dtos;
using Abp.BackgroundJobs;
using System.Diagnostics;
using MachineLearningBP.Shared.Framework;
using System.Runtime.CompilerServices;

namespace MachineLearningBP.Core.Services
{
    public class SheetUtilityDomainService : BaseDomainService, ISheetUtilityDomainService
    {
        #region Properties
        private const String GoogleSheetMimeType = "application/vnd.google-apps.spreadsheet";
        private const string CLIENT_ID = "554760688430-p13ivih9tnv97gpbgan1h28k9dem8fll.apps.googleusercontent.com";
        private const string CLIENT_SECRET = "kS7N359vgNTNUvj1FPGrI5Sk";
        private const string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.file";
        private const string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
        private const string AppName = "Machine Learning";

        private readonly string[] Scopes = { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
        #endregion

        #region Constructor
        public SheetUtilityDomainService(ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager) : base(sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
        }
        #endregion

        #region Record
        public async Task Record(List<IRecordContainer> results, [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "")
        {
            try
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create("Writing results to Google..."));

                String combineFileAndMember = Extensions.CombineFileAndMember(filePath, memberName);
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "MachineLearningBP.Files.client_secret.json";

                string credPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/machine-learning.json");

                UserCredential credential;
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(assembly.GetManifestResourceStream(resourceName)).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true));

                // Create the service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = AppName,
                });

                Google.Apis.Drive.v2.Data.File newSheet = new Google.Apis.Drive.v2.Data.File();
                DateTime now = DateTime.Now;
                newSheet.Title = String.Format("{0:M/d/yyyy h:mm:ss tt}", now);
                newSheet.Description = $"{combineFileAndMember} ran at {now}";
                newSheet.MimeType = GoogleSheetMimeType;

                var mth = new StackFrame(1).GetMethod();
                newSheet.Parents = new List<ParentReference>() { new ParentReference() { Id = this._settingManager.GetSettingValue($"DriveFolder.{combineFileAndMember}") } };

                FilesResource.InsertRequest newSheetRequestInsert = service.Files.Insert(newSheet);
                Google.Apis.Drive.v2.Data.File newSheetRef = newSheetRequestInsert.Execute();

                OAuth2Parameters parameters = GetOauth2Parameters();
                parameters.AccessToken = _settingManager.GetSettingValue("GoogleAccessToken");
                parameters.RefreshToken = _settingManager.GetSettingValue("GoogleRefreshToken");

                SpreadsheetsService spreadsheetsService = new SpreadsheetsService(AppName);
                GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, AppName, parameters);
                spreadsheetsService.RequestFactory = requestFactory;

                SpreadsheetQuery spreadsheetQuery = new SpreadsheetQuery();
                spreadsheetQuery.NumberToRetrieve = 1;
                spreadsheetQuery.StartDate = now;
                SpreadsheetFeed feed = spreadsheetsService.Query(spreadsheetQuery);

                SpreadsheetEntry spreadsheet = (SpreadsheetEntry)feed.Entries[0];

                WorksheetEntry worksheet = (WorksheetEntry)spreadsheet.Worksheets.Entries[0];
                //worksheet.Title = new AtomTextConstruct(AtomTextConstructElementType.Title, String.Format("Bulk Insert Benchmark - {0}", benchmark.TotalRecords));
                //worksheet.Update();

                CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
                CellFeed cellFeed = spreadsheetsService.Query(cellQuery);
                CellEntry cellEntry = new CellEntry();

                uint currentRow = 1;
                uint currentColumn = 1;

                IRecordContainer firstResult = results.First();

                foreach(String columnHeader in firstResult.ColumnHeaders)
                {
                    cellEntry = new CellEntry(currentRow, currentColumn, columnHeader);
                    cellFeed.Insert(cellEntry);
                    currentColumn++;
                }

                currentRow++;

                foreach (IRecordContainer result in results)
                {
                    currentColumn = 1;

                    foreach (String columnValue in result.ColumnValues)
                    {
                        cellEntry = new CellEntry(currentRow, currentColumn, columnValue);
                        cellFeed.Insert(cellEntry);
                        currentColumn++;
                    }

                    currentRow++;
                }

                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Click for <a href='{spreadsheet.AlternateUri.Content}' target='_blank'>Results</a>"));
            }
            catch (Exception ex)
            {
                this._consoleHubProxy.WriteLine(ConsoleWriteLineInput.Create($"Exception: {ex.Message} {Environment.NewLine} Stacktrace: {ex.StackTrace}"));
            }
        }
        #endregion

        #region GetAccessToken
        public String GetAccessTokenUrl()
        {
            OAuth2Parameters parameters = GetOauth2Parameters();

            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);

            return authorizationUrl;
        }
        #endregion

        #region CompleteGetAccessToken
        public void CompleteGetAccessToken(GetAccessTokenInput input)
        {
            OAuth2Parameters parameters = GetOauth2Parameters();

            parameters.AccessCode = input.AccessCode;

            OAuthUtil.GetAccessToken(parameters);
            _settingManager.ChangeSettingForApplication("GoogleAccessToken", parameters.AccessToken);
            _settingManager.ChangeSettingForApplication("GoogleRefreshToken", parameters.RefreshToken);
        }
        #endregion

        #region GetOauth2Parameters
        private static OAuth2Parameters GetOauth2Parameters()
        {
            OAuth2Parameters parameters = new OAuth2Parameters();
            parameters.ClientId = CLIENT_ID;
            parameters.ClientSecret = CLIENT_SECRET;
            parameters.RedirectUri = REDIRECT_URI;
            parameters.Scope = SCOPE;

            return parameters;
        }
        #endregion
    }
}
