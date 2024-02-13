using System.Collections;
using System.Data;

namespace Library.Api.Data
{
	public interface IRepository<TEntity> where TEntity : class
	{
		Task<IEnumerable<TEntity>> GetAll(object? searchCriteria, int? page, int? pageSize);
		Task<TEntity?> GetById(string id);
		Task<TEntity> Insert(TEntity entity);
		Task<bool> Update(TEntity entity);
		Task ExecuteAsync(string sql, object? parameters = null);

	}
	internal class Repository<TEntity>(
		IDbConnectionFactory dbConnectionFactory,
		ILogger<Repository<TEntity>> logger) : IRepository<TEntity> where TEntity : class
	{
		private readonly IDbConnection _db = dbConnectionFactory.Connection;
		private readonly ILogger<Repository<TEntity>> _logger = logger;

		public async Task<IEnumerable<TEntity>> GetAll(object? searchCriteria, int? page, int? pageSize)
		{
			page ??= 1;
			pageSize ??= 20;
			string tableName = typeof(TEntity).Name;
			int offset = (int)((page - 1) * pageSize);
			string columns = GetColumnNames<TEntity>();
			string condition = searchCriteria != null ? BuildSearchCondition(searchCriteria) : string.Empty;

			string query = $"""
			                SELECT {columns}
			                FROM {tableName}
			                {condition}
			                ORDER BY Id
			                OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY
			                """;

			_logger.LogInformation("Executing: {Query}", query);

			return await _db.QueryAsync<TEntity>(query, searchCriteria);
		}

		public async Task<TEntity?> GetById(string id)
		{
			string tableName = typeof(TEntity).Name;
			string columns = GetColumnNames<TEntity>();
			string query = $"""
			                SELECT {columns}
			                FROM {tableName}
			                WHERE Id = @Id
			                """;

			_logger.LogInformation("Executing {query}", query);

			return await _db.QueryFirstOrDefaultAsync<TEntity>(query, 
				new
				{
					Id = id
				});
		}
		public async Task<TEntity> Insert(TEntity entity)
		{
			string tableName = typeof(TEntity).Name;
			var propertyNames = GetPropertyNames(entity);
			string columnNamesString = string.Join(", ", propertyNames);
			string valueParameters = string.Join(", ", propertyNames.Select(prop => $"@{prop}"));

			string query = $"""
			                INSERT INTO {tableName} ({columnNamesString})
			                OUTPUT INSERTED.*
			                VALUES ({valueParameters})
			                """;

			_logger.LogInformation("Executing: {query} ", query);
			
			TEntity insertedEntity = await _db.QuerySingleAsync<TEntity>(query, entity);

			return insertedEntity;
		}
		public async Task<bool> Update(TEntity entity)
		{
			string tableName = typeof(TEntity).Name;
			string propertyNames = string.Join(", ", GetPropertyNames(entity).Select(p => $"{p}=@{p}"));

			string query = $"""
			                UPDATE {tableName}
			                SET {propertyNames}
			                WHERE Id = @Id
			                """;
			
			_logger.LogInformation("Executing: {query}", query);

			return await _db.ExecuteAsync(query, entity) > 0;
		}

		public async Task ExecuteAsync(string sql, object? parameters = null)
		{
			_logger.LogInformation("Executing: {query}", sql);
			await _db.ExecuteAsync(sql, parameters);
		}

		protected static string BuildSearchCondition(object searchCriteria)
		{
			var properties = searchCriteria.GetType().GetProperties();
			var conditions = properties.Select(prop => $"{prop.Name} LIKE '%{prop.GetValue(searchCriteria)}%'");
			return $"WHERE {string.Join(" AND ", conditions)}";
		}
		private static string GetColumnNames<T>()
		{
			var columns = typeof(T).GetProperties()
				.Where(prop => !IsCollectionOrObjectType(prop.PropertyType))
				.Select(prop => prop.Name);

			return string.Join(", ", columns);
		}
		private static IEnumerable<string> GetPropertyNames<T>(T entity)
		{
			return typeof(T)
				.GetProperties()
				.Where(prop => !IsNullOrEmpty(prop.GetValue(entity)))
				.Select(prop => prop.Name);
		}
		private static bool IsCollectionOrObjectType(Type type)
			=> type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>) 
			   || type == typeof(object);
		private static bool IsNullOrEmpty(object? value) 
			=> value == null || (value is IEnumerable enumerable && !enumerable.Cast<object>().Any());

	}
}
