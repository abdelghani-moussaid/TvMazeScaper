using Moq;
using Moq.Protected;
using TvMazeScaper.Data;
using TvMazeScaper.Services;
using Xunit;
using Microsoft.EntityFrameworkCore;

public class TvMazeServiceTests
{
    [Fact]
    public async Task ProcessShowAsync_ReturnsCorrectCountAndShows()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("[{\"id\":1,\"name\":\"Test Show 1\"}, {\"id\":2,\"name\":\"Test Show 2\"}, {\"id\":3,\"name\":\"Test Show 3\"}]"),
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var options = new DbContextOptionsBuilder<TvMazeContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var fakeContext = new TvMazeContext(options);
        var service = new TvMazeService(httpClient, fakeContext);

        // Act
        var result = await service.ProcessShowAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // Ensure the correct count is returned
        Assert.Contains(result, s => s.Name == "Test Show 1");
        Assert.Contains(result, s => s.Name == "Test Show 2");
        Assert.Contains(result, s => s.Name == "Test Show 3");
    }
    
    [Fact]
    public async Task ProcessCastAsync_ReturnsCorrectCast()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("[{\"person\":{\"id\":1,\"name\":\"Test Person 1\",\"birthday\":\"1980-01-01\"}}, {\"person\":{\"id\":2,\"name\":\"Test Person 2\",\"birthday\":\"1990-02-02\"}}, {\"person\":{\"id\":3,\"name\":\"Test Person 3\",\"birthday\":\"2000-03-03\"}}]"),
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var options = new DbContextOptionsBuilder<TvMazeContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var fakeContext = new TvMazeContext(options);
        var service = new TvMazeService(httpClient, fakeContext);

        // Act
        var result = await service.ProcessCastAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // Ensure the correct count is returned
        Assert.Contains(result, c => c.Person.Name == "Test Person 1" && c.Person.Birthday == new DateTime(1980, 1, 1));
        Assert.Contains(result, c => c.Person.Name == "Test Person 2" && c.Person.Birthday == new DateTime(1990, 2, 2));
        Assert.Contains(result, c => c.Person.Name == "Test Person 3" && c.Person.Birthday == new DateTime(2000, 3, 3));
    }

    [Fact]
    public async Task PersistShowsAsync_SavesShowsToDatabase()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("[{\"id\":1,\"name\":\"Test Show 1\"}, {\"id\":2,\"name\":\"Test Show 2\"}, {\"id\":3,\"name\":\"Test Show 3\"}]"),
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var options = new DbContextOptionsBuilder<TvMazeContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var fakeContext = new TvMazeContext(options);
        var service = new TvMazeService(httpClient, fakeContext);

        // Act
        var result = await service.PersistShowsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count); // Ensure the correct count is returned
        Assert.Contains(result, s => s.Name == "Test Show 1");
        Assert.Contains(result, s => s.Name == "Test Show 2");
        Assert.Contains(result, s => s.Name == "Test Show 3");

        // Verify data persistence
        var showsInDb = await fakeContext.Show.ToListAsync();
        Assert.Equal(3, showsInDb.Count);
        Assert.Contains(showsInDb, s => s.Name == "Test Show 1");
        Assert.Contains(showsInDb, s => s.Name == "Test Show 2");
        Assert.Contains(showsInDb, s => s.Name == "Test Show 3");
    }

    [Fact]
    public async Task PersistCastAsync_SavesCastToDatabase()
    {
        // Arrange
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("[{\"person\":{\"id\":1,\"name\":\"Test Person 1\",\"birthday\":\"1980-01-01\"}}, {\"person\":{\"id\":2,\"name\":\"Test Person 2\",\"birthday\":\"1990-02-02\"}}, {\"person\":{\"id\":3,\"name\":\"Test Person 3\",\"birthday\":\"2000-03-03\"}}]"),
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var options = new DbContextOptionsBuilder<TvMazeContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        var fakeContext = new TvMazeContext(options);
        var service = new TvMazeService(httpClient, fakeContext);

        // Act
        await service.PersistCastAsync(1);

        // Assert
        var castInDb = await fakeContext.Cast.Include(c => c.Person).ToListAsync();
        Assert.Equal(3, castInDb.Count); // Ensure the correct count is returned
        Assert.Contains(castInDb, c => c.Person.Name == "Test Person 1" && c.Person.Birthday == new DateTime(1980, 1, 1));
        Assert.Contains(castInDb, c => c.Person.Name == "Test Person 2" && c.Person.Birthday == new DateTime(1990, 2, 2));
        Assert.Contains(castInDb, c => c.Person.Name == "Test Person 3" && c.Person.Birthday == new DateTime(2000, 3, 3));
    }
}
