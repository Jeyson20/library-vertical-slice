using Library.Api.DTOs.Responses;

namespace Library.Api.Features.Authors
{
	public static class GetAuthorById
	{
		public record Query(string AuthorId) : IRequest<Result<AuthorResponse>>;

		internal sealed class Handler(IDatabaseService databaseService) : IRequestHandler<Query, Result<AuthorResponse>>
		{
			private readonly IDatabaseService _databaseService = databaseService;
			public async Task<Result<AuthorResponse>> Handle(Query request, CancellationToken cancellationToken)
			{
				try
				{
					var author = await _databaseService
						.GetById<Author>(request.AuthorId);

					if (author is null)
					{
						return Result<AuthorResponse>.NotFound();
					}

					var authorResponse = author.Adapt<AuthorResponse>();

					return Result.Success(authorResponse);
				}
				catch (SqlException error)
				{
					return Result<AuthorResponse>.Failure(
						Errors.DatabaseError(nameof(Author), error.Message));
				}
			}
		}
	}

	public class GetAuthorByIdEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("api/authors/{authorId}", async (string authorId, ISender sender) =>
			{

				var result = await sender.Send(
					new GetAuthorById.Query(authorId));

				return result.HandleResult();

			}).WithTags("Authors");
		}
	}
}
