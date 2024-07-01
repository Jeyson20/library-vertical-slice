using Library.Api.DTOs.Responses;
using Library.Api.Entities;
using Library.Api.Shared.Models;
using Mapster;

namespace Library.UnitTests.Authors;

public class GetAllAuthorsTests
{
    [Fact]
    public async Task GetAllAuthors_ShouldReturnPaginatedOfAuthorResponseCollection()
    {
        //Arrange
        var query = new GetAllAuthors.Query(default, default, default);
        (var authors, int totalCount, int pageSize, int pageNumber) = GetAuthors();
        
        var authorRepositoryMock = new Mock<IAuthorRepository>();
        authorRepositoryMock.Setup(x => 
                x.GetAllAuthors(It.IsAny<string?>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ReturnsAsync((authors, totalCount, pageSize, pageNumber));

        var handler = new GetAllAuthors.Handler(authorRepositoryMock.Object);
        
        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        
        //Assert
        Assert.NotEmpty(result.Items);
    }

    private static (IEnumerable<Author>, int, int, int) GetAuthors()
    {
        var author1 = Author.Create("test1", "test1", new DateTime(1990, 12, 12));
        var author2 = Author.Create("test2", "test2", new DateTime(1990, 12, 12));
        var authors = new List<Author> { author1, author2 };
        return (authors, 2, 20, 1);
    }
}