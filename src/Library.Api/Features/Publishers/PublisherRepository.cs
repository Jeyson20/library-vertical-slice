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
		                   INSERT INTO Publisher (Id, Name)
		                   OUTPUT INSERTED.*
		                   VALUES (@Id, @Name)
		                   """;
		
		_logger.LogInformation("Executing: {Sql}", sql);
		
		return await _dbConnection.Connection.QuerySingleAsync<Publisher>(sql, new
		{
			publisher.Id,
			publisher.Name
		});
	}
	public async Task<IEnumerable<Publisher>> GetAll(CancellationToken cancellationToken = default)
	{
		const string sql = """
		                   SELECT Id, Name
		                   FROM Publisher
		                   """;
		
		_logger.LogInformation("Executing: {Sql}", sql);
		
		return await _dbConnection.Connection.QueryAsync<Publisher>(sql, cancellationToken);
	}
}
