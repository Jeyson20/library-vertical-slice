namespace Library.Api.Shared.Models;

public class ValidationResult(IReadOnlyCollection<Error> errors)
{
	public IReadOnlyCollection<Error> Errors { get; } = errors;
}
