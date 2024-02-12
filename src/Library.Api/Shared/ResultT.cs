using System.Net;

namespace Library.Api.Shared;

public class Result<TValue> : Result
{
	private readonly TValue _value;
	public bool HasValue => !HasNoValue;
	private bool HasNoValue => _value is null;

	protected internal Result(TValue value, bool isSuccess, Error error, HttpStatusCode statusCode)
		: base(isSuccess, error, statusCode)
		=> _value = value;

	public static Result<TValue> None => new(default!, default, default!, default);

	public new static Result<TValue> Failure(Error error)
	{
		return new Result<TValue>(default!, false, error, HttpStatusCode.BadRequest);
	}

	public static Result<TValue> NotFound(Error error)
	{
		return new Result<TValue>(default!, false, error, HttpStatusCode.NotFound);
	}

	public static Result<TValue> NotFound()
	{
		return new Result<TValue>(default!, false, default!, HttpStatusCode.NotFound);
	}
	public TValue Value => IsSuccess
		? _value
		: throw new InvalidOperationException("The value of a failure result cannot be accessed.");

	public static implicit operator Result<TValue>(TValue value) => Success(value);
}
