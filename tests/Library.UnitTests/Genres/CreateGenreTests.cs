using Library.Api.Entities;
using Library.Api.Features.Genres;

namespace Library.UnitTests.Genres;

public class CreateGenreTests
{
    [Fact]
    public async Task CreateGenre_ShouldReturnCreatedGenre()
    {
        // Arrange
        var request = new CreateGenre.Command("Fantasy", "Description");
        var expectedGenre = Genre.Create("Fantasy", "Description");

        var genreRepositoryMock = new Mock<IGenreRepository>();
        genreRepositoryMock.Setup(x => x.Exists(request.Name))
            .ReturnsAsync(false);
        genreRepositoryMock.Setup(x => x.Create(It.IsAny<Genre>()))
            .ReturnsAsync(expectedGenre);

        var handler = new CreateGenre.Handler(genreRepositoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }
}