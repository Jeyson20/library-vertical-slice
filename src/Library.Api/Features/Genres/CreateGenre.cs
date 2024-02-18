using Library.Api.DTOs.Responses;

namespace Library.Api.Features.Genres;

public static class CreateGenre
{
    public record Command(string Name, string? Description) : IRequest<Result<GenreResponse>>;

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(p => p.Name).NotEmpty().WithError(GenresErrors.NameIsRequired);
        }
    }

    internal sealed class Handler(IGenreRepository repository) : IRequestHandler<Command, Result<GenreResponse>>
    {
        private readonly IGenreRepository _repository = repository;

        public async Task<Result<GenreResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var newGenre = Genre.Create(
                request.Name,
                request.Description);

            bool genreExists = await _repository.Exists(request.Name);
            if (genreExists)
                return Result<GenreResponse>.Failure(GenresErrors.AlreadyExists);

            Genre genre = await _repository.Create(newGenre);
            var genreResponse = genre.Adapt<GenreResponse>();

            return Result.Success(genreResponse);
        }
    }
}

public class CreateGenreRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class CreateGenreEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/genres", async (ISender sender, CreateGenreRequest request) =>
        {
            var result = await sender.Send(new CreateGenre.Command(
                request.Name,
                request.Description));
            
            return result.HandleResult();
        }).Produces<GenreResponse>()
        .WithTags("Genres")
        .WithSummary("Create genre.")
        .WithOpenApi();
    }
}