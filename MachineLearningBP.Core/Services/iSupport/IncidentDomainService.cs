using MachineLearningBP.CollectiveIntelligence.DomainServices.Samples;
using MachineLearningBP.Entities.iSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Repositories;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.SqlExecuter;
using System.Data.SqlClient;
using System.Data;
using MachineLearningBP.Framework;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;

namespace MachineLearningBP.Services.iSupport
{
    public class IncidentDomainService : MinimumSampleDomainService<Incident, IncidentStatLine>, IIncidentDomainService
    {
        #region Properties
        //String connectionString = "Server=dev-csandfort\\MASTER;Database=cSupport;integrated security=true;";
        String connectionString = "Server=production.gwi.com;Database=cSupport;integrated security=true;";

        String countSql = @"SELECT COUNT(*)
                            FROM [cSupport].[dbo].INCIDENTS as inc
                            INNER JOIN [cSupport].[dbo].INCIDENT_STATUSES as incStatus
                            ON inc.ID_STATUS = incStatus.ID
                            WHERE 
                            incStatus.[TYPE] = 2 AND (SELECT SUM(RWH.TIME_WORKED) FROM REP_WORK_HISTORY AS RWH WHERE RWH.INCIDENT_NUMBER = inc.NUMBER) < {0}";

        String selectSql = @"SELECT TOP({0})
                            inc.ID,
                            CAST(ID_CUSTOMER as nvarchar(max)) as Customer,
                            CAST(ID_COMPANY as nvarchar(max)) as Company,
                            CAST([SOURCE] as nvarchar(max)) as Source,
                            CAST(ID_TEMPLATE as nvarchar(max)) as Template,
                            MONTH(DT_CREATED) as 'Month',
                            DATEPART(WEEKDAY, DT_CREATED) as DayOfWeek,
                            (SELECT TOP(1) [PRIORITY] FROM PRIORITY_TIMELINE_EVENTS WHERE ID_INCIDENT = inc.ID GROUP BY [PRIORITY] ORDER BY SUM([TIME_IN_STATE]) DESC) as 'Priority',
                            CAST((SELECT TOP(1) ID_INCIDENT_STATUS FROM STATUS_TIMELINE_EVENTS WHERE ID_INCIDENT = inc.ID GROUP BY [ID_INCIDENT_STATUS] ORDER BY SUM([TIME_IN_STATE]) DESC) as nvarchar(max))  as 'Status',
                            CAST((SELECT TOP(1) ID_ASSIGNEE FROM ASSIGNEE_TIMELINE_EVENTS WHERE ID_INCIDENT = inc.ID GROUP BY [ID_ASSIGNEE] ORDER BY SUM([TIME_IN_STATE]) DESC) as nvarchar(max)) as 'Assignee',
                            (SELECT REPLACE((SELECT REPLACE((SELECT STUFF((SELECT ID FROM fn_GetCategoryAncestry((SELECT TOP(1) ID_CATEGORY FROM CATEGORY_TIMELINE_EVENTS WHERE ID_INCIDENT = inc.ID GROUP BY [ID_CATEGORY] ORDER BY SUM([TIME_IN_STATE]) DESC)) FOR XML PATH('')), 1, 0, '')), '<ID>', '')), '</ID>', '_')) as 'Category',
                            (SELECT SUM(RWH.TIME_WORKED) FROM REP_WORK_HISTORY AS RWH WHERE RWH.INCIDENT_NUMBER = inc.NUMBER) as 'TotalTimeWorked'

                            FROM [cSupport].[dbo].INCIDENTS as inc
                            INNER JOIN [cSupport].[dbo].INCIDENT_STATUSES as incStatus
                            ON inc.ID_STATUS = incStatus.ID

                            WHERE 
                            incStatus.[TYPE] = 2 AND inc.[ID] > {1}
                            AND (SELECT SUM(RWH.TIME_WORKED) FROM REP_WORK_HISTORY AS RWH WHERE RWH.INCIDENT_NUMBER = inc.NUMBER) < {2}
                            
                            ORDER BY inc.[ID]";

        int pageSize = 100;
        int maxPages = 100000000;
        int maxResult = 500;
        #endregion

        public IncidentDomainService(IRepository<Incident> sampleRepository, IRepository<IncidentStatLine> statLineRepository,
            ISqlExecuter sqlExecuter, IConsoleHubProxy consoleHubProxy, ISettingManager settingManager, IBackgroundJobManager backgroundJobManager)
            : base(sampleRepository, statLineRepository, sqlExecuter, consoleHubProxy, settingManager, backgroundJobManager)
        {
        }

        public void DeleteSamples()
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                using (var unitOfWork = this.UnitOfWorkManager.Begin())
                {
                    this._sqlExecuter.Execute("DELETE FROM [Incidents]");
                    unitOfWork.Complete();
                }
            }
        }

        public async Task PopulateSamples()
        {
            using (GuerillaTimer guerillaTimer = new GuerillaTimer(this._consoleHubProxy))
            {
                this.DeleteSamples();

                using (EtlAssistant<Incident> dbLoader = new EtlAssistant<Incident>(connectionString, countSql, selectSql, pageSize, maxPages, maxResult,
                    Incident.GetListFromDataReader, x => this._sampleRepository.Insert(x), this.UnitOfWorkManager, this._consoleHubProxy))
                {
                    await dbLoader.Load();
                }
            }
        }
    }
}
