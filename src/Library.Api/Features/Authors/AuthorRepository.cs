using Library.Api.Data;

namespace Library.Api.Features.Authors;

public interface IAuthorRepository : IRepository<Author>
{
	Task<(IEnumerable<Author> Authors,
		int TotalCount,
		int PageSize,
		int PageNumber)> GetAllAuthors(string? firstName, int? pageSize, int? pageNumber);
}
internal class AuthorRepository(
	IDbConnectionFactory dbConnectionFactory,
	ILogger<Repository<Author>> logger) : Repository<Author>(dbConnectionFactory, logger), IAuthorRepository
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
}
