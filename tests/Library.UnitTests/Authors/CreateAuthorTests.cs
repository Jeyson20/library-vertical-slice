using Library.Api.Data;
using Library.Api.Entities;

namespace Library.UnitTests.Authors
{
	public class CreateAuthorTests
	{
		[Fact]
		public void CreateAuthor_ShouldReturnValidationError_WhenFirstNameIsNullOrEmpty()
		{
			//Arrange
			var validator = new CreateAuthor.Validator();
			var command = new CreateAuthor.Command("", "test", new DateTime(2000, 01, 01));

			//Act
			var validationResult = validator.Validate(command);

			//Assert
			Assert.False(validationResult.IsValid);
			Assert.Contains(nameof(CreateAuthor.Command.FirstName), validationResult.Errors.Select(e => e.PropertyName));
		}

		[Fact]
		public async Task CreateAuthor_ShouldReturnCreatedAuthor()
		{
			//Arrange
			var command = new CreateAuthor.Command("test", "test", new DateTime(2000, 01, 01));
			var expectedAuthor = Author.Create("test", "test", new DateTime(2000, 01, 01));
			
			var repositoryMock = new Mock<IAuthorRepository>();
			repositoryMock.Setup(x => x.Insert(It.IsAny<Author>()))
				.ReturnsAsync(expectedAuthor);
			
			var handler = new CreateAuthor.Handler(repositoryMock.Object);
			
			//Act
			var result = await handler.Handle(command, CancellationToken.None);

			//Assert
			Assert.True(result.IsSuccess);
			Assert.NotNull(result.Value);
		}
	}
}
