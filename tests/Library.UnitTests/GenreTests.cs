using Library.Api.Entities;
using Library.Api.Features.Genres;

namespace Library.UnitTests;

public class GenreTests
{
    [Fact]
    public async Task Handle_ValidRequest_ShouldCreateGenre()
    {
        // Arrange
        var request = new CreateGenre.Command("Fantasy", "Description");
        var expectedGenre = new Genre { Name = "Fantasy", Description = "Description" };
            
        var genreRepositoryMock = new Mock<IGenreRepository>();
        genreRepositoryMock.Setup(x => x.Exists(request.Name)).ReturnsAsync(false);
        genreRepositoryMock.Setup(x => x.Create(It.IsAny<Genre>())).ReturnsAsync(expectedGenre);
            
        var handler = new CreateGenre.Handler(genreRepositoryMock.Object);
            
        // Act
        var result = await handler.Handle(request, CancellationToken.None);
            
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Fantasy", result.Value.Name);
    }
}