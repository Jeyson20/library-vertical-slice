using System.Data;
using Library.Api.Data;

namespace Library.Api.Features.Genres;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAll();
    Task<Genre> Create(Genre genre);
    Task<bool> Exists(string name);
}

internal class GenreRepository(
    IDbConnectionFactory dbConnectionFactory,
    ILogger<GenreRepository> logger) : IGenreRepository
{
    private readonly IDbConnection _db = dbConnectionFactory.Connection;
    private readonly ILogger<GenreRepository> _logger = logger;

    public async Task<IEnumerable<Genre>> GetAll()
    {
        string query = """
                       SELECT Id, Name, Description
                       FROM Genre
                       ORDER BY Name
                       """;
        _logger.LogInformation("Executing: {Query}", query);

        return await _db.QueryAsync<Genre>(query);
    }

    public async Task<Genre> Create(Genre genre)
    {
        string query = """
                       INSERT INTO Genre(Id, Name, Description)
                       OUTPUT INSERTED.*
                       VALUES(@Id, @Name, @Description)
                       """;
        _logger.LogInformation("Executing: {query} ", query);
        return await _db.QuerySingleAsync<Genre>(query, genre);
    }

    public async Task<bool> Exists(string name)
    {
        string query = """
                       SELECT COUNT(*)
                       FROM Genre
                       WHERE Name = @Name

                       """;
        _logger.LogInformation("Executing: {query} ", query);
        
        int count = await _db.QuerySingleAsync<int>(query, new {Name = name});
        return count > 0;
    }
}