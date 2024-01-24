using System.Data;

namespace Library.Api.Shared.Interfaces
{
	public interface IDatabaseConnectionFactory
	{
		IDbConnection Connection { get; }
	}
}
