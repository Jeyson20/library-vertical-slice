namespace Library.Api.Features.Genres;

public static class GenresErrors
{
    internal static Error NameIsRequired => new($"Genre.NameIsRequired", "The genre name is required.");
    internal static Error AlreadyExists => new($"Genre.AlreadyExists", "The genre already exists.");
}