namespace Library.Api.Shared.Interfaces
{
	public interface IDatabaseService
	{
		Task<IEnumerable<T>> GetAll<T>(string tableName, object? searchCriteria = null, int page = 1, int pageSize = 20);
		Task<T?> GetById<T>(string id);
		Task<int> Insert<T>(T entity);
		Task<bool> Update<T>(T entity);
		Task ExecuteAsync(string sql, object? parameters = null);

	}
}
