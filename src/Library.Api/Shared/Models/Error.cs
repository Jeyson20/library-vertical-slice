namespace Library.Api.Shared;

public sealed class Error(string code, string message)
{
	public string Code { get; } = code;
	public string Message { get; } = message;
	internal static Error None => new(string.Empty, string.Empty);

	public static implicit operator string(Error error) => error?.Code ?? string.Empty;
}
