using System.Data;

namespace Library.Api.Database;

internal class SqlConnectionFactory(string connectionString) : IDatabaseConnectionFactory
{
	private readonly string _connectionString = connectionString;

	public IDbConnection Connection => new SqlConnection(_connectionString);
}
