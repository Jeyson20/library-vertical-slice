using Library.Api.Data;

namespace Library.Api.Features.Authors;

public interface IAuthorRepository : IRepository<Author>;

internal class AuthorRepository(
	IDbConnectionFactory dbConnectionFactory,
	ILogger<Repository<Author>> logger) :  Repository<Author>(dbConnectionFactory, logger), IAuthorRepository
{
	private readonly IDbConnectionFactory _connectionFactory = dbConnectionFactory;
	private readonly ILogger<Repository<Author>> _logger = logger;
	
	
}
