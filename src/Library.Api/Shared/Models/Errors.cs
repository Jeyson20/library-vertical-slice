namespace Library.Api.Shared.Models;

public static class Errors
{
	internal static Error ServerError(string message) => new("API.ServerError", message);
	internal static Error DatabaseError(string message) => new($"API.SqlError", message);
	internal static Error ValidationError(Type type, string message) => new($"{type.Name}.Validation", message);
}
