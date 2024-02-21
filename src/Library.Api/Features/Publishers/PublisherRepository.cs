using Library.Api.Data;

namespace Library.Api.Features.Publishers;

public interface IPublisherRepository
{
	Task<Publisher> Create(Publisher publisher, CancellationToken cancellationToken = default);
	Task<IEnumerable<Publisher>> GetAll(CancellationToken cancellationToken = default);
}
public class PublisherRepository(
	IDbConnectionFactory dbConnection,
	ILogger<PublisherRepository> logger) : IPublisherRepository
{
	private readonly IDbConnectionFactory _dbConnection = dbConnection;
	private readonly ILogger<PublisherRepository> _logger = logger;
	public async Task<Publisher> Create(Publisher publisher, CancellationToken cancellationToken = default)
	{
		const string sql = """
		                   INSERT INTO Publishers (Id, Name)
		                   OUTPUT INSERTED.*
		                   VALUES (@Id, @Name)
		                   """;
		return await _dbConnection.Connection.QuerySingleAsync<Publisher>(sql, publisher);
	}
	public async Task<IEnumerable<Publisher>> GetAll(CancellationToken cancellationToken = default)
	{
		const string sql = """
		                   SELECT Id, Name
		                   FROM Publishers
		                   """;
		return await _dbConnection.Connection.QueryAsync<Publisher>(sql, cancellationToken);
	}
}
