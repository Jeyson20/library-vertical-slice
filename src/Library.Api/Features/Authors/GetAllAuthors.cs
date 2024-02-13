using Library.Api.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Features.Authors;

public static class GetAllAuthors
{
	public record Query(
		string? FirstName,
		int? PageNumber,
		int? PageSize) : IRequest<Paginated<AuthorResponse>>;

	internal sealed class Handler(IAuthorRepository repository) : IRequestHandler<Query, Paginated<AuthorResponse>>
	{
		private readonly IAuthorRepository _repository = repository;

		public async Task<Paginated<AuthorResponse>> Handle(Query request, CancellationToken cancellationToken)
		{
			(var authors, int totalCount, int pageSize, int pageNumber) = await _repository.GetAllAuthors(
				request.FirstName,
				request.PageSize,
				request.PageNumber
			);

			var authorResponse = authors.Adapt<ICollection<AuthorResponse>>();

			return new Paginated<AuthorResponse>(authorResponse, totalCount, pageNumber, pageSize);
		}
	}
}
public class GetAllAuthorsEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("api/authors", async (
				ISender sender,
				string? firstName,
				int? pageNumber,
				int? pageSize) =>
			{
				var result = await sender.Send(
					new GetAllAuthors.Query(
						firstName,
						pageNumber,
						pageSize));

				return Results.Ok(result);

			}).Produces<Paginated<AuthorResponse>>()
			.WithTags("Authors")
			.WithSummary("Get all authors.")
			.WithOpenApi();
	}
}
