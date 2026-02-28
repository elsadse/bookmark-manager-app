using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace BookmarkManagerApp.IntegrationTests.Controllers;

public class UserControllerTest(BookmarkManagerAppFactory factory) : IClassFixture<BookmarkManagerAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private const string BaseUrl = "/api/users";

    [Fact]
    public async Task GetUserById_withNonExistent_ReturnsNotFound()
    {
        //Arrange
        using var GetUserByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/123");

        // Act
        using var GetUserByIdResponse = await _client.SendAsync(GetUserByIdRequest, _cancellationToken);

        // Assert
        GetUserByIdResponse.Should().Be404NotFound();
    }

    [Fact]
    public async Task GetUserById_ReturnsOk()
    {
        //Arrange
        using var GetUserByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/1");

        // Act
        using var GetUserByIdResponse = await _client.SendAsync(GetUserByIdRequest, _cancellationToken);

        // Assert
        GetUserByIdResponse.Should().Be200Ok()
            .And.BeAs(new
            {
                Fullname = "Test User",
                Email = "test.user@example.com"
            });
    }

}