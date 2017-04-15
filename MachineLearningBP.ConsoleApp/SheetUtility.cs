using Abp.Configuration;
using Abp.Dependency;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Util.Store;
using MachineLearningBP.Services.Sports.Nba;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.Dtos;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MachineLearningBP.ConsoleApp
{
    public class SheetUtility : ITransientDependency, IDisposable
    {
        #region Properties
        private const String GoogleSheetMimeType = "application/vnd.google-apps.spreadsheet";
        private const string CLIENT_ID = "554760688430-p13ivih9tnv97gpbgan1h28k9dem8fll.apps.googleusercontent.com";
        private const string CLIENT_SECRET = "kS7N359vgNTNUvj1FPGrI5Sk";
        private const string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.file";
        private const string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

        private readonly string[] Scopes = { DriveService.Scope.DriveFile, DriveService.Scope.Drive };
        #endregion


        #region GetAccessToken
        public void Authorize()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MachineLearningBP.ConsoleApp.Files.client_secret.json";

            string credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            credPath = Path.Combine(credPath, ".credentials/machine-learning.json");

            UserCredential credential;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(assembly.GetManifestResourceStream(resourceName)).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;

            Console.WriteLine("Credential file saved to: " + credPath);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
        } 
        #endregion
    }
}
