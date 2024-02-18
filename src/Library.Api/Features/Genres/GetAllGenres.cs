using Library.Api.DTOs.Responses;

namespace Library.Api.Features.Genres;

public static class GetAllGenres
{
    public record Query : IRequest<ICollection<GenreResponse>>;

    internal sealed class Handler(IGenreRepository repository) : IRequestHandler<Query, ICollection<GenreResponse>>
    {
        private readonly IGenreRepository _repository = repository;

        public async Task<ICollection<GenreResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var genres = await _repository.GetAll();
            
            var genresResponse = genres.Adapt<ICollection<GenreResponse>>();
            
            return genresResponse;
        }
    }
}

public class GetAllGenresEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/genres", async (ISender sender)  =>
        {
            var result = await sender.Send(new GetAllGenres.Query());
            return result;
        }).Produces<ICollection<GenreResponse>>()
        .WithTags("Genres")
        .WithSummary("Get all genres.")
        .WithOpenApi();
    }
}

