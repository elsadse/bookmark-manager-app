using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace BookmarkManagerApp.IntegrationTests.Controllers;

public class TagControllerTest: IClassFixture<BookmarkManagerAppFactory>
{
    private const string BaseUrl = "/api/tags";
    private readonly HttpClient _client;
    private readonly CancellationToken _cancellationToken;
    private readonly Utility _utility;
    private readonly (string Email, string Password) _seedUser;

    public TagControllerTest(BookmarkManagerAppFactory factory)
    {
        _client = factory.CreateClient();
        _cancellationToken = CancellationToken.None;
        _utility = new Utility(_client, _cancellationToken);
        _seedUser = BookmarkManagerAppFactory.getSeedUser();
    }

    [Fact]
    public async Task RetrieveAllTag_ReturnsOk()
    {
        // Arrange
        using var RetrieveAllTagRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}");
        RetrieveAllTagRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var RetrieveAllTagResponse = await _client.SendAsync(RetrieveAllTagRequest, _cancellationToken);

        //Assert
        RetrieveAllTagResponse.Should().Be200Ok()
            .And.BeAs(new[]
        {
            new  { TagId = 1, Name = "Tools" },
            new  { TagId = 2, Name = "Community" },
            new  { TagId = 3, Name = "Git" }
        });
    }

    [Fact]
    public async Task RetrieveAllTag_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        using var RetrieveAllTagRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}");

        // Act
        using var RetrieveAllTagResponse = await _client.SendAsync(RetrieveAllTagRequest, _cancellationToken);

        // Assert
        RetrieveAllTagResponse.Should().Be401Unauthorized();
    }
}