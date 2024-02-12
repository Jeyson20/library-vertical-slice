namespace Library.Api.Shared;

public class ValidationResult(IReadOnlyCollection<Error> errors)
{
	public IReadOnlyCollection<Error> Errors { get; } = errors;
}
