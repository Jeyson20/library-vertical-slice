using System.Data;

namespace Library.Api.Database;

internal class SqlConnectionManager(string connectionString) : IDatabaseConnection
{
	private readonly string _connectionString = connectionString;
	public IDbConnection Connection => new SqlConnection(_connectionString);

}
