using System.Net;

namespace Library.Api.Shared;

public class Result
{
	public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;
	public Error Error { get; }
	public HttpStatusCode StatusCode { get; }

	protected Result(bool isSuccess, Error error, HttpStatusCode statusCode)
	{
		IsSuccess = isSuccess;
		Error = error;
		StatusCode = statusCode;
	}
	public static Result Success() => new(true, Error.None, HttpStatusCode.OK);
	public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None, HttpStatusCode.OK);
	public static Result Failure(Error error) => new(false, error, HttpStatusCode.BadRequest);
	public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error, HttpStatusCode.BadRequest);
}