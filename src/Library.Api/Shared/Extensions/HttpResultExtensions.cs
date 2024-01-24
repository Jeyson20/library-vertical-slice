using System.Net;

namespace Library.Api.Shared.Extensions;

public static class HttpResultExtensions
{
	public static IResult HandleResult<TIn>(this Result<TIn> result)
		=> result.IsSuccess ? Results.Ok(result.Value) : HandleStatusCode(result);

	public static IResult HandleResult(this Result result)
		=> result.IsSuccess ? Results.Ok() : HandleStatusCode(result);

	private static IResult HandleStatusCode(Result result)
	{
		return result.StatusCode switch
		{
			HttpStatusCode.BadRequest => Results.BadRequest(result.Error),
			HttpStatusCode.NotFound => Results.NotFound(),
			_ => Results.BadRequest()
		};
	}
}
