using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MachineLearningBP.CollectiveIntelligence.Entities;
using MachineLearningBP.Entities;
using MachineLearningBP.Shared;
using MachineLearningBP.Shared.Dtos;
using MachineLearningBP.Shared.GuerillaTimer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningBP.Framework
{
    public class EtlAssistant<TSample> : IDisposable
        where TSample: IDbLoadable
    {
        public String ConnectionString { get; set; }
        public String CountSql { get; set; }
        public String SelectSql { get; set; }
        public int PageSize { get; set; }
        public int MaxPages { get; set; }
        public int MaxResult { get; set; }
        public Func<IDataReader, List<TSample>> GetListFromDataReader { get; set; }
        public Action<TSample> Insert { get; set; }
        public IUnitOfWorkManager UnitOfWorkManager { get; set; }
        public readonly IConsoleHubProxy _consoleHubProxy;

        SqlConnection Connection = new SqlConnection();
        public int Count = 0;
        public int PageCount = 0;
        public int LastId = 0;

        public EtlAssistant()
        {
        }

        public EtlAssistant(String connectionString, String countSql, String selectSql, int pageSize, int maxPages, int maxResult,
            Func<IDataReader, List<TSample>> getListFromDataReader, Action<TSample> insert,
            IUnitOfWorkManager unitOfWorkManager, IConsoleHubProxy consoleHubProxy)
        {
            this.ConnectionString = connectionString;
            this.CountSql = countSql;
            this.SelectSql = selectSql;
            this.PageSize = pageSize;
            this.MaxPages = maxPages;
            this.MaxResult = maxResult;
            this.GetListFromDataReader = getListFromDataReader;
            this.Insert = insert;
            this.UnitOfWorkManager = unitOfWorkManager;
            this._consoleHubProxy = consoleHubProxy;

            this.Connection = new SqlConnection(this.ConnectionString);
            this.Connection.Open();
        }

        public async Task Load()
        {
            SqlCommand countCmd = new SqlCommand();
            countCmd.CommandText = String.Format(this.CountSql, this.MaxResult);
            countCmd.CommandType = CommandType.Text;
            countCmd.Connection = this.Connection;
            Count = (int)countCmd.ExecuteScalar();
            this.PageCount = Math.Min(this.MaxPages, (int)Math.Ceiling((double)this.Count / (double)this.PageSize));

            for(int i = 0; i < this.PageCount; i++)
            {
                using (GuerillaTimer timer = new GuerillaTimer(this._consoleHubProxy, $"ETL page {i + 1} of {this.PageCount}"))
                {

                    SqlCommand selectCmd = new SqlCommand();
                    selectCmd.CommandText = String.Format(this.SelectSql, this.PageSize, this.LastId, this.MaxResult);
                    selectCmd.CommandType = CommandType.Text;
                    selectCmd.Connection = this.Connection;

                    SqlDataReader reader = await selectCmd.ExecuteReaderAsync();
                    List<TSample> samples = this.GetListFromDataReader(reader);
                    reader.Close();

                    if (samples.Count > 0)
                    {
                        using (var unitOfWork = this.UnitOfWorkManager.Begin())
                        {
                            foreach (TSample sample in samples)
                            {
                                this.Insert(sample);
                            }
                            await unitOfWork.CompleteAsync();
                        }

                        this.LastId = samples.Last().LastId;
                    }
                }
            }
        }

        public void Dispose()
        {
            this.Connection.Close();
        }
    }
}
