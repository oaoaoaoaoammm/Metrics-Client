namespace WorkWithBD
{
	public interface IRepository<T> where T : class
	{
		IList<T> GetAll();
		T GetById(int id);
		void Create(T item);
		void Update(T item);
		void Delete(int id);
		IList<T> GetMetricsByTimePeriod(DateTime one, DateTime two);
	}
}

