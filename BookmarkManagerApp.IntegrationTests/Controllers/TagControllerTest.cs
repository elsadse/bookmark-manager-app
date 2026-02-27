using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace BookmarkManagerApp.IntegrationTests.Controllers;

public class TagControllerTest(BookmarkManagerAppFactory factory) : IClassFixture<BookmarkManagerAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private const string BaseUrl = "/api/tags";

    private async Task<string> LoginAndGetJwtCookieAsync()
    {
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/api/auth/login");
        var loginRequestData = new
        {
            Email = "test.user@example.com",
            Password = "Pass123!"
        };
        loginRequest.Content = JsonContent.Create(loginRequestData);

        using var loginResponse = await _client.SendAsync(loginRequest, _cancellationToken);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        return loginResponse.Headers.TryGetValues("Set-Cookie", out var values)
            ? values.First()
            : throw new InvalidOperationException("Login response did not contain Set-Cookie header.");
    }

    [Fact]
    public async Task RetrieveAllTag_ReturnsOk()
    {
        // Arrange
        using var RetrieveAllTagRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}");
        RetrieveAllTagRequest.Headers.Add("Cookie", await LoginAndGetJwtCookieAsync());

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