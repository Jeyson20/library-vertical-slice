namespace Library.Api.Features.Authors;

public static class DeleteAuthor
{
	public record Query(string AuthorId) : IRequest<Result>;

	internal sealed class Handler(IAuthorRepository repository) : IRequestHandler<Query, Result>
	{
		private readonly IAuthorRepository _repository = repository;

		public async Task<Result> Handle(Query request, CancellationToken cancellationToken)
		{
			var author = await _repository.GetById(request.AuthorId);
			if (author is null)
				return Result.NotFound();

			await _repository.Delete(request.AuthorId);
			return Result.Success();
		}
	}

}
public class DeleteAuthorEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("api/authors/{authorId}", async (
				string authorId,
				ISender sender,
				CancellationToken cancellationToken) =>
			{
				var result = await sender.Send(new DeleteAuthor.Query(authorId),
					cancellationToken);

				return result.HandleResult();
			}).WithSummary("Delete author.")
			.WithTags("Authors")
			.WithOpenApi();
	}
}
