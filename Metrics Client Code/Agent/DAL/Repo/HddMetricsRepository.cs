using System.Data.SQLite;
using Dapper;
using WorkWithBD;
using Microsoft.OpenApi.Models;

namespace MetricsAgent
{
	public class HddMetricsRepository : IHddMetricsRepository
	{
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";


        public void Create(HddMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO hddmetrics(value, time) VALUES(@value, @time)",
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
                connection.Execute("DELETE FROM hddmetrics WHERE id=@id", new
                {
                    id = id
                });
            }
        }


        public void Update(HddMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE hddmetrics SET value = @value, time = @time WHERE id=@id", new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
            }
        }


        public IList<HddMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<HddMetrics>("SELECT Id, Time, Value FROM hddmetrics").ToList();
            }
        }


        public HddMetrics GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<HddMetrics>("SELECT Id, Time, Value FROM hddmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }


        public IList<HddMetrics> GetMetricsByTimePeriod(DateTime fromTime, DateTime toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<HddMetrics>("SELECT * FROM hddmetrics WHERE Time >= @fromTime AND Time <= @toTime",
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime
                    }).ToList();
            }
        }
    }
}

