using System.Data.SQLite;
using Dapper;
using WorkWithBD;
using Microsoft.OpenApi.Models;

namespace MetricsAgent
{
	public class DotNetMetricsRepository : IDotNetMetricsRepository
	{
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";


        public void Create(DotNetMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO dotnetmetrics(value, time) VALUES(@value, @time)",
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
                connection.Execute("DELETE FROM dotnetmetrics WHERE id=@id", new
                {
                    id = id
                });
            }
        }


        public void Update(DotNetMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE dotnetmetrics SET value = @value, time = @time WHERE id=@id", new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
            }
        }


        public IList<DotNetMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetrics>("SELECT Id, Time, Value FROM dotnetmetrics").ToList();
            }
        }


        public DotNetMetrics GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<DotNetMetrics>("SELECT Id, Time, Value FROM dotnetmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }


        public IList<DotNetMetrics> GetMetricsByTimePeriod(DateTime fromTime, DateTime toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<DotNetMetrics>("SELECT * FROM dotnetmetrics WHERE Time >= @fromTime AND Time <= @toTime",
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime
                    }).ToList();
            }
        }
    }
}

