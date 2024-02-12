using Library.Api.Shared.Middleware;

namespace Library.Api.Shared.Extensions;

public static class MiddlewareExtensions
{
	public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<ExceptionHandlerMiddleware>();
	}
}
