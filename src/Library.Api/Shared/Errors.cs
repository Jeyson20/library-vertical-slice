namespace Library.Api.Shared;

public static class Errors
{
	internal static Error ServerError(string message) => new("API.ServerError", message);
	internal static Error DatabaseError(string name, string message) => new($"{name}.SqlError", message);
	internal static Error ValidationError(Type type, string message) => new($"{type.Name}.Validation", message);
}
