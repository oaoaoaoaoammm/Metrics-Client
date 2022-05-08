using System.Data.SQLite;
using Dapper;
using Microsoft.OpenApi.Models;

namespace WorkWithBD
{
    public class CpuMetricsRepository : ICpuMetricsRepository
    {
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";


        public void Create(CpuMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO cpumetrics(value, time) VALUES(@value, @time)",
                 new
                {
                      value = item.Value,
                       time = item.Time
                 });
            }
        }


        public void Delete(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("DELETE FROM cpumetrics WHERE id=@id", new
                {
                    id = id
                });
            }
        }


        public void Update(CpuMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE cpumetrics SET value = @value, time = @time WHERE id=@id", new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
            }
         }


        public IList<CpuMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetrics>("SELECT Id, Time, Value FROM cpumetrics").ToList();
            }
        }


        public CpuMetrics GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<CpuMetrics>("SELECT Id, Time, Value FROM cpumetrics WHERE id=@id",
                    new {
                        id = id
                    });
            }
        }


        public IList<CpuMetrics> GetMetricsByTimePeriod(DateTime fromTime, DateTime toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<CpuMetrics>("SELECT * FROM cpumetrics WHERE Time >= @fromTime AND Time <= @toTime",
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime
                    }).ToList();
            }
        }
    }
}

