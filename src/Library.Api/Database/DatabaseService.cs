
using System.Collections;
using System.Data;

namespace Library.Api.Database
{
	public class DatabaseService(IDatabaseConnection dbConnection, ILogger<DatabaseService> logger) : IDatabaseService
	{
		private readonly IDbConnection _dbConnection = dbConnection.Connection;
		private readonly ILogger<DatabaseService> _logger = logger;

		public async Task<IEnumerable<T>> GetAll<T>(string tableName, object? searchCriteria = null, int page = 1, int pageSize = 10)
		{
			var offset = (page - 1) * pageSize;
			var columns = GetColumnNames<T>();
			var condition = searchCriteria != null ? BuildSearchCondition(searchCriteria) : string.Empty;

			var query = $@"
            SELECT {columns} 
            FROM {tableName}
            {condition}
            ORDER BY Id
            OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";

			return await _dbConnection.QueryAsync<T>(query, searchCriteria);
		}

		public async Task<T?> GetById<T>(string id)
		{
			var tableName = typeof(T).Name;
			var columns = GetColumnNames<T>();
			string query = $"SELECT {columns} FROM {tableName} WHERE Id = @Id";

			_logger.LogInformation("Executing {query}", query);

			return await _dbConnection
				.QueryFirstOrDefaultAsync<T>(query, new
				{
					Id = id
				});
		}
		public async Task<int> Insert<T>(T entity)
		{
			var tableName = typeof(T).Name;
			var propertyNames = GetPropertyNames(entity);
			var columnNamesString = string.Join(", ", propertyNames);
			var valueParameters = string.Join(", ", propertyNames.Select(prop => $"@{prop}"));

			string query = $"INSERT INTO {tableName} ({columnNamesString}) VALUES ({valueParameters})";

			_logger.LogInformation("Executing: {query} ", query);

			return await _dbConnection.ExecuteAsync(query, entity);
		}

		public async Task<bool> Update<T>(T entity)
		{
			var tableName = nameof(T);
			var propertyNames = string.Join(", ", GetPropertyNames(entity).Select(p => $"{p}=@{p}"));

			string query = $"UPDATE {tableName} SET {propertyNames} WHERE Id = @Id";

			_logger.LogInformation("Executing: {query}", query);

			return await _dbConnection.ExecuteAsync(query, entity) > 0;
		}
		public async Task ExecuteAsync(string sql, object? parameters = null)
		{
			_logger.LogInformation("Executing: {query}", sql);
			await _dbConnection.ExecuteAsync(sql, parameters);
		}

		private static string BuildSearchCondition(object searchCriteria)
		{
			var properties = searchCriteria.GetType().GetProperties();
			var conditions = properties.Select(prop => $"{prop.Name} LIKE @{prop.Name}");
			return $"WHERE {string.Join(" AND ", conditions)}";
		}
		private static string GetColumnNames<T>()
		{
			var columns = typeof(T).GetProperties()
				.Where(prop => !IsCollectionOrObjectType(prop.PropertyType))
				.Select(prop => prop.Name);

			return string.Join(", ", columns);
		}

		private static bool IsCollectionOrObjectType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>) ||
				type == typeof(object);
		}
		private static IEnumerable<string> GetPropertyNames<T>(T entity)
		{
			return typeof(T)
				.GetProperties()
				.Where(prop => !IsNullOrEmpty(prop.GetValue(entity)))
				.Select(prop => prop.Name);
		}
		private static bool IsNullOrEmpty(object? value) => value == null || (value is IEnumerable enumerable && !enumerable.Cast<object>().Any());

	}
}
