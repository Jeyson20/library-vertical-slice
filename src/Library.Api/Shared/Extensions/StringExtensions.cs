namespace Library.Api.Shared.Extensions;

public static class StringExtensions
{
	public static bool HasValue(this string? param) => !string.IsNullOrEmpty(param) && !string.IsNullOrWhiteSpace(param);
}
