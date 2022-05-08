using System.Data.SQLite;
using Dapper;
using WorkWithBD;
using Microsoft.OpenApi.Models;

namespace MetricsAgent
{
	public class NetworkMetricsRepository : INetworkMetricsRepository
	{
        private const string ConnectionString = "Data Source=metrics.db;Version=3;Pooling=true;Max Pool Size=100;";


        public void Create(NetworkMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("INSERT INTO networkmetrics(value, time) VALUES(@value, @time)",
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
                connection.Execute("DELETE FROM networkmetrics WHERE id=@id", new
                {
                    id = id
                });
            }
        }


        public void Update(NetworkMetrics item)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Execute("UPDATE networkmetrics SET value = @value, time = @time WHERE id=@id", new
                {
                    value = item.Value,
                    time = item.Time,
                    id = item.Id
                });
            }
        }


        public IList<NetworkMetrics> GetAll()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<NetworkMetrics>("SELECT Id, Time, Value FROM networkmetrics").ToList();
            }
        }


        public NetworkMetrics GetById(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.QuerySingle<NetworkMetrics>("SELECT Id, Time, Value FROM networkmetrics WHERE id=@id",
                    new
                    {
                        id = id
                    });
            }
        }


        public IList<NetworkMetrics> GetMetricsByTimePeriod(DateTime fromTime, DateTime toTime)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                return connection.Query<NetworkMetrics>("SELECT * FROM networkmetrics WHERE Time >= @fromTime AND Time <= @toTime",
                    new
                    {
                        fromTime = fromTime,
                        toTime = toTime
                    }).ToList();
            }
        }
    }
}

