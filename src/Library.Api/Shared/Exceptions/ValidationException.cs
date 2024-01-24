
using FluentValidation.Results;

namespace Library.Api.Shared.Exceptions;

public sealed class ValidationException(IEnumerable<ValidationFailure> failures)
: Exception("One or more validation failures have ocurred.")
{
	public IReadOnlyCollection<Error> Errors { get; } = failures
		.Distinct()
		.Select(group => new Error(group.ErrorCode, group.ErrorMessage))
		.ToArray();
}
