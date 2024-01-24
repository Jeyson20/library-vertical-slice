namespace Library.Api.Features.Authors;

public static class AuthorErrors
{
	internal static Error FirstNameIsRequired => new($"Author.FirstNameIsRequired", "The author firstName is required.");
	internal static Error LastNameIsRequired => new($"Author.LastNameIsRequired", "The author lastName is required.");
	internal static Error DateOfBirthIsRequired => new($"Author.DateOfBirthIsRequired", "The author dateOfBirth is required.");
}
