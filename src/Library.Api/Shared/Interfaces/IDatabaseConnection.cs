using System.Data;

namespace Library.Api.Shared.Interfaces
{
	public interface IDatabaseConnection
	{
		IDbConnection Connection { get; }
	}
}
