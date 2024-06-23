using Library.Api.Data;

namespace Library.Api.Features.Authors;

public interface IAuthorRepository
{
	Task<(IEnumerable<Author> Authors,
		int TotalCount,
		int PageSize,
		int PageNumber)> GetAllAuthors(string? firstName, int? pageSize, int? pageNumber);
	Task<Author?> GetById(string authorId);
	Task<Author> Insert(Author author);
	Task<bool> Delete(string authorId);


}
internal class AuthorRepository(
	IDbConnectionFactory dbConnectionFactory,
	ILogger<Repository<Author>> logger) : IAuthorRepository
{
	private readonly IDbConnectionFactory _connectionFactory = dbConnectionFactory;
	private readonly ILogger<Repository<Author>> _logger = logger;

	public async Task<(
		IEnumerable<Author> Authors,
		int TotalCount,
		int PageSize,
		int PageNumber)> GetAllAuthors(string? firstName, int? pageSize, int? pageNumber)
	{
		pageNumber ??= 1;
		pageSize ??= 20;
		int offSet = (int)((pageNumber - 1) * pageSize);

		object? parameters = firstName.HasValue()
			? new { firstName }
			: default;

		string condition = parameters != null ? BuildSearchCondition(parameters) : string.Empty;

		string query = $"""
		                SELECT (SELECT COUNT(*)
		                        FROM Author {condition}) As TotalCount;
		                SELECT Id, FirstName, LastName, DateOfBirth
		                FROM Author {condition}
		                ORDER BY Id ASC
		                OFFSET {offSet} ROWS FETCH NEXT {pageSize} ROWS ONLY;
		                """;
		_logger.LogInformation("Executing: {Query}", query);

		var result = await _connectionFactory.Connection
			.QueryMultipleAsync(query);

		int totalCount = await result.ReadFirstAsync<int>();
		var authors = await result.ReadAsync<Author>();

		return (authors, totalCount, (int)pageSize, (int)pageNumber);
	}
	public async Task<Author?> GetById(string authorId)
	{
		const string query = $"""
		                      SELECT Id, FirstName, LastName, DateOfBirth
		                      FROM Author
		                      WHERE Id = @Id
		                      """;
		return await _connectionFactory.Connection
			.QueryFirstOrDefaultAsync<Author>(query, new { Id = authorId });
	}

	public async Task<Author> Insert(Author author)
	{
		const string query = $"""
		                      INSERT INTO Author (Id,FirstName, LastName, DateOfBirth)
		                      OUTPUT INSERTED.*
		                      VALUES (@Id, @FirstName, @LastName, @DateOfBirth)
		                      """;

		_logger.LogInformation("Executing: {query} ", query);

		Author authorInserted = await _connectionFactory.Connection
			.QuerySingleAsync<Author>(query, author);

		return authorInserted;
	}
	public async Task<bool> Delete(string authorId)
	{
		const string query = """
		                     DELETE FROM Author
		                     WHERE Id = @Id
		                     """;
		return await _connectionFactory.Connection.ExecuteAsync( query, new { Id = authorId }) > 0;
	}

	private static string BuildSearchCondition(object searchCriteria)
	{
		var properties = searchCriteria.GetType().GetProperties();
		var conditions = properties.Select(prop => $"{prop.Name} LIKE '%{prop.GetValue(searchCriteria)}%'");
		return $"WHERE {string.Join(" AND ", conditions)}";
	}
}
