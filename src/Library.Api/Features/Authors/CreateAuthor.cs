using Library.Api.DTOs.Requests;
using Library.Api.DTOs.Responses;

namespace Library.Api.Features.Authors
{
	public static class CreateAuthor
	{
		public record class Command(string FirstName, string LastName, DateTime DateOfBirth) : IRequest<Result<AuthorResponse>>;
		public class Validator : AbstractValidator<Command>
		{
			public Validator()
			{
				RuleFor(p => p.FirstName).NotEmpty().WithError(AuthorErrors.FirstNameIsRequired);
				RuleFor(p => p.LastName).NotEmpty().WithError(AuthorErrors.LastNameIsRequired);
				RuleFor(p => p.DateOfBirth).NotNull().WithError(AuthorErrors.DateOfBirthIsRequired);

				//.WithError(AuthorErrors.PropertyIsRequired(nameof(Author.DateOfBirth)));
			}
		}

		internal sealed class Handler(IAuthorRepository repository) : IRequestHandler<Command, Result<AuthorResponse>>
		{
			private readonly IAuthorRepository _repository = repository;
			public async Task<Result<AuthorResponse>> Handle(Command request, CancellationToken cancellationToken)
			{
				var newAuthor = Author.Create(
					request.FirstName,
					request.LastName,
					request.DateOfBirth);

				var author = await _repository.Insert(newAuthor);

				var response = author.Adapt<AuthorResponse>();

				return Result.Success(response);
			}
		}
	}

	public class CreateAuthorEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapPost("api/authors", async (CreateAuthorRequest request, ISender sender) =>
			{
				var command = request.Adapt<CreateAuthor.Command>();

				var result = await sender.Send(command);

				return result.HandleResult();

			}).Produces<AuthorResponse>()
			.WithTags("Authors")
			.WithSummary("Create author.")
			.WithOpenApi();
		}
	}
}
