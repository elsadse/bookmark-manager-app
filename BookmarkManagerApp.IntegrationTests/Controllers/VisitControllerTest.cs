using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace BookmarkManagerApp.IntegrationTests.Controllers;

public class VisitControllerTest(BookmarkManagerAppFactory factory) : IClassFixture<BookmarkManagerAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private const string BaseUrl = "/api/visits";

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
    public async Task GetVisitById_Unauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        using var GetVisitByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/123");

        // Act
        using var GetVisitByIdResponse = await _client.SendAsync(GetVisitByIdRequest, _cancellationToken);

        // Assert
        GetVisitByIdResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task GetVisitById_withNonExistent_ReturnsNotFound()
    {
        //Arrange
        using var GetVisitByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/123");
        GetVisitByIdRequest.Headers.Add("Cookie", await LoginAndGetJwtCookieAsync());

        // Act
        using var GetVisitByIdResponse = await _client.SendAsync(GetVisitByIdRequest, _cancellationToken);

        // Assert
        GetVisitByIdResponse.Should().Be404NotFound();
    }

    [Fact]
    public async Task GetVisitById_ReturnsOk()
    {
        //Arrange
        using var GetVisitByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/1");
        GetVisitByIdRequest.Headers.Add("Cookie", await LoginAndGetJwtCookieAsync());

        // Act
        using var GetVisitByIdResponse = await _client.SendAsync(GetVisitByIdRequest, _cancellationToken);

        // Assert
        GetVisitByIdResponse.Should().Be200Ok()
            .And.BeAs(new
            {
                BookmarkId = 1,
                VisitTime = DateTimeOffset.Parse("2026-02-27T09:00:00Z")
            });
    }

    [Fact]
    public async Task CreateVisit_Unauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        using var createVisitRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        var createVisitData = new
        {
            BookmarkId = 1,
            VisitTime = DateTime.UtcNow
        };
        createVisitRequest.Content = JsonContent.Create(createVisitData);

        //Act
        using var CreateVisitResponse = await _client.SendAsync(createVisitRequest, _cancellationToken);

        // Assert
        CreateVisitResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task CreateVisit_WithExistingData_ReturnsConflict()
    {
        //Arrange
        using var createVisitRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        createVisitRequest.Headers.Add("Cookie", await LoginAndGetJwtCookieAsync());
        var createVisitData = new
        {
            BookmarkId = 1,
            VisitTime = DateTimeOffset.Parse("2026-02-27T09:00:00Z")
        };
        createVisitRequest.Content = JsonContent.Create(createVisitData);

        //Act
        using var CreateVisitResponse = await _client.SendAsync(createVisitRequest, _cancellationToken);

        // Assert
        CreateVisitResponse.Should().Be409Conflict();
    }

    [Fact]
    public async Task CreateVisit_WithInvalidData_ReturnsBadRequest()
    {
        //Arrange
        using var createVisitRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        createVisitRequest.Headers.Add("Cookie", await LoginAndGetJwtCookieAsync());
        var createVisitData = new
        {
            BookmarkId = 0,
            VisitTime = DateTimeOffset.Parse("2024-02-27T09:00:00Z")
        };
        createVisitRequest.Content = JsonContent.Create(createVisitData);

        //Act
        using var CreateVisitResponse = await _client.SendAsync(createVisitRequest, _cancellationToken);

        // Assert
        CreateVisitResponse.Should().Be400BadRequest()
            .And.BeAs(new
            {
                title = "Validation Error",
                status = 400,
                errors = new
                {
                    BookmarkId = (string[])["'Bookmark Id' must be greater than '0'."],
                    VisitTime = (string[])["'Visit Time' must be within the last year or in the future."]
                }
            });
    }

    [Fact]
    public async Task CreateVisit_WithValidData_ReturnsCreated()
    {
        //Arrange
        using var createVisitRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        createVisitRequest.Headers.Add("Cookie", await LoginAndGetJwtCookieAsync());
        var createVisitData = new
        {
            BookmarkId = 2,
            VisitTime = DateTimeOffset.Parse("2026-02-27T09:00:00Z")
        };
        createVisitRequest.Content = JsonContent.Create(createVisitData);

        //Act
        using var CreateVisitResponse = await _client.SendAsync(createVisitRequest, _cancellationToken);

        // Assert
        CreateVisitResponse.Should().Be201Created()
            .And.BeAs(new
            {
                BookmarkId = 2,
                VisitTime = DateTimeOffset.Parse("2026-02-27T09:00:00Z")
            })
            .And.HaveHeader("Location")
            .And.Match($"http://localhost{BaseUrl}/2");

    }


}
