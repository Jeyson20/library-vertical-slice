using System.Data;

namespace Library.Api.Data
{
	public interface IDbConnectionFactory
	{
		IDbConnection Connection { get; }
	}
	
	internal class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
	{
		private readonly string _connectionString = connectionString;
		public IDbConnection Connection => new SqlConnection(_connectionString);

	}
}
