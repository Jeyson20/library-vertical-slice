using Microsoft.AspNetCore.Http.HttpResults;

namespace Library.Api.Shared.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
	private readonly RequestDelegate _next = next;

	public async Task Invoke(HttpContext ctx)
	{
		try
		{
			await _next(ctx);
		}
		catch (Shared.Exceptions.ValidationException ex)
		{
			await Results.UnprocessableEntity(new ValidationResult(ex.Errors)).ExecuteAsync(ctx);
		}
		catch (SqlException ex)
		{
			await Results.BadRequest(Errors.DatabaseError(ex.Message)).ExecuteAsync(ctx);
		}
		catch (Exception ex)
		{
			await Results.Problem(Errors.ServerError(ex.Message)).ExecuteAsync(ctx);
		}
	}
}
