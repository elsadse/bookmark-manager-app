using System.Net.Http.Json;
using FluentAssertions;

namespace BookmarkManagerApp.IntegrationTests.Controllers;

public class BookmarkControllerTest : IClassFixture<BookmarkManagerAppFactory>, IAsyncLifetime
{
    private readonly BookmarkManagerAppFactory _factory;
    private const string BaseUrl = "/api/bookmarks";
    private readonly HttpClient _client;
    private readonly CancellationToken _cancellationToken;
    private readonly Utility _utility;
    private readonly (string Email, string Password) _seedUser;

    public BookmarkControllerTest(BookmarkManagerAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _cancellationToken = CancellationToken.None;
        _utility = new Utility(_client, _cancellationToken);
        _seedUser = BookmarkManagerAppFactory.getSeedUser();
    }

    public async ValueTask InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    [Fact]
    public async Task CreateBookmark_Unauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        using var createBookmarkRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        var createBookmarkData = new
        {
            Title = "Stack Overflow"
        };
        createBookmarkRequest.Content = JsonContent.Create(createBookmarkData);

        //Act
        using var CreateBookmarkResponse = await _client.SendAsync(createBookmarkRequest, _cancellationToken);

        // Assert
        CreateBookmarkResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task CreateBookmark_WithExistingData_ReturnsConflict()
    {
        //Arrange
        using var createBookmarkRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        createBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var createBookmarkData = new
        {
            Title = "Stack Overflow",
            Url = "https://stackoverflow.com",
            Description = "The largest, most trusted online community for developers to learn, share their knowledge, and build their careers.",
            Tags = new List<string> { "Reference", "Community", "Tips" }
        };
        createBookmarkRequest.Content = JsonContent.Create(createBookmarkData);

        //Act
        using var CreateBookmarkResponse = await _client.SendAsync(createBookmarkRequest, _cancellationToken);

        // Assert
        CreateBookmarkResponse.Should().Be409Conflict();
    }

    [Fact]
    public async Task CreateBookmark_WithInvalidData_ReturnsBadRequest()
    {
        //Arrange
        using var createBookmarkRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        createBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var createBookmarkData = new
        {
            Title = "",
            Url = "",
            Description = "",
            Tags = new List<string> { }
        };
        createBookmarkRequest.Content = JsonContent.Create(createBookmarkData);

        //Act
        using var CreateBookmarkResponse = await _client.SendAsync(createBookmarkRequest, _cancellationToken);

        // Assert
        CreateBookmarkResponse.Should().Be400BadRequest()
            .And.BeAs(new
            {
                title = "Validation Error",
                status = 400,
                errors = new Dictionary<string, string[]>
                {
                    ["Title"] = [
                        "'Title' must not be empty.",
                        "The length of 'Title' must be at least 3 characters. You entered 0 characters."
                    ],
                    ["Url"] = ["The specified condition was not met for 'Url'."],
                    ["Description"] = ["'Description' must not be empty."],
                    ["Tags"] = ["A bookmark can have at least one tag and at most 20 unique tags."]
                }
            });
    }

    [Fact]
    public async Task CreateBookmark_WithInvalidDataAndSameTag_ReturnsBadRequest()
    {
        //Arrange
        using var createBookmarkRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        createBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var createBookmarkData = new
        {
            Title = "Ts",
            Url = "https://example.com",
            Description = "Create Description Test",
            Tags = new List<string> { "Reference", "refeRENCE" }
        };
        createBookmarkRequest.Content = JsonContent.Create(createBookmarkData);

        //Act
        using var CreateBookmarkResponse = await _client.SendAsync(createBookmarkRequest, _cancellationToken);

        // Assert
        CreateBookmarkResponse.Should().Be400BadRequest()
            .And.BeAs(new
            {
                title = "Validation Error",
                status = 400,
                errors = new Dictionary<string, string[]>
                {
                    ["Title"] = ["The length of 'Title' must be at least 3 characters. You entered 2 characters."],
                    ["Tags"] = ["Tags must be unique."]
                }
            });
    }

    [Fact]
    public async Task CreateBookmark_WithValidData_ReturnsOk()
    {
        //Arrange
        using var createBookmarkRequest = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}");
        createBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var createBookmarkData = new
        {
            Title = "MDN Web Docs",
            Url = "https://developer.mozilla.org",
            Description = "The MDN Web Docs site provides information about Open Web technologies including HTML, CSS, and APIs for both Web sites and progressive web apps.",
            Tags = new List<string> { "Reference", "HTML", "CSS", "JavaScript" }
        };
        createBookmarkRequest.Content = JsonContent.Create(createBookmarkData);

        //Act
        using var CreateBookmarkResponse = await _client.SendAsync(createBookmarkRequest, _cancellationToken);

        // Assert
        CreateBookmarkResponse.Should().Be201Created()
            .And.BeAs(new
            {
                Title = "MDN Web Docs",
                Url = "https://developer.mozilla.org",
                Description = "The MDN Web Docs site provides information about Open Web technologies including HTML, CSS, and APIs for both Web sites and progressive web apps.",
                Tags = new List<string> { "Reference", "HTML", "CSS", "JavaScript" }
            })
            .And.HaveHeader("Location")
            .And.Match($"http://localhost{BaseUrl}/3");
    }

    [Fact]
    public async Task GetBookmarkById_Unauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        using var getBookmarkByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/123");

        // Act
        using var getBookmarkByIdResponse = await _client.SendAsync(getBookmarkByIdRequest, _cancellationToken);

        // Assert
        getBookmarkByIdResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task GetBookmarkById_withNonExistent_ReturnsNotFound()
    {
        //Arrange
        using var getBookmarkByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/123");
        getBookmarkByIdRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var getBookmarkByIdResponse = await _client.SendAsync(getBookmarkByIdRequest, _cancellationToken);

        // Assert
        getBookmarkByIdResponse.Should().Be404NotFound();
    }

    [Fact]
    public async Task GetBookmarkById_ReturnsOk()
    {
        //Arrange
        using var getBookmarkByIdRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/1");
        getBookmarkByIdRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var getBookmarkByIdResponse = await _client.SendAsync(getBookmarkByIdRequest, _cancellationToken);

        // Assert
        getBookmarkByIdResponse.Should().Be200Ok()
            .And.BeAs(new
            {
                Title = "GitHub",
                Url = "https://github.com",
                Description = "Where the world builds software. Millions of developers and companies build, ship, and maintain their software on GitHub.",
                Tags = new List<string> { "Community", "Git" }
            });
    }

    [Fact]
    public async Task GetAllBookmark_ReturnsOk()
    {
        // Arrange
        using var getAllBookmarkRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}");
        getAllBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var getAllBookmarkResponse = await _client.SendAsync(getAllBookmarkRequest, _cancellationToken);

        //Assert
        getAllBookmarkResponse.Should().Be200Ok()
            .And.BeAs(new[]
        {
            new{
                bookmarkId= 1,
                title = "GitHub",
                url = "https://github.com",
                description = "Where the world builds software. Millions of developers and companies build, ship, and maintain their software on GitHub.",
                isPinned     = false,
                isArchived   = true,
                tags = new List<string> { "Community", "Git" },
                visitsCount  = 1,
                lastVisitTime = DateTimeOffset.Parse("2026-02-27T09:00:00Z") as DateTimeOffset?
            },
            new{
                bookmarkId= 2,
                title = "Stack Overflow",
                url = "https://stackoverflow.com",
                description = "The largest, most trusted online community for developers to learn, share their knowledge, and build their careers.",
                isPinned     = false,
                isArchived   = false,
                tags = new List<string> {"Tools", "Community" },
                visitsCount  = 0,
                lastVisitTime = null as DateTimeOffset?
            }
        }, options => options
            .ExcludingMissingMembers()
            .Excluding(info => info.Path.EndsWith("creationTime"))
        );
    }

    [Fact]
    public async Task GetAllBookmark_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        using var getAllBookmarkRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}");

        // Act
        using var getAllBookmarkResponse = await _client.SendAsync(getAllBookmarkRequest, _cancellationToken);

        // Assert
        getAllBookmarkResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task TooglePinBookmark_withNonExistent_ReturnsNotFound()
    {
        //Arrange
        using var tooglePinBookmarkRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/123/pin");
        tooglePinBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var tooglePinBookmarkResponse = await _client.SendAsync(tooglePinBookmarkRequest, _cancellationToken);

        // Assert
        tooglePinBookmarkResponse.Should().Be404NotFound();
    }

    [Fact]
    public async Task TooglePinBookmark_withExisting_ReturnsForbid()
    {
        //Arrange
        using var tooglePinBookmarkRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/1/pin");
        tooglePinBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var tooglePinBookmarkResponse = await _client.SendAsync(tooglePinBookmarkRequest, _cancellationToken);

        // Assert
        tooglePinBookmarkResponse.Should().Be403Forbidden();
    }

    [Fact]
    public async Task TooglePinBookmark_ReturnsOk()
    {
        //Arrange
        using var tooglePinBookmarkRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/2/pin");
        tooglePinBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var tooglePinBookmarkResponse = await _client.SendAsync(tooglePinBookmarkRequest, _cancellationToken);

        // Assert
        tooglePinBookmarkResponse.Should().Be204NoContent();
    }

    [Fact]
    public async Task TooglePinBookmark_Unauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        using var tooglePinBookmarkRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/1/pin");

        // Act
        using var tooglePinBookmarkResponse = await _client.SendAsync(tooglePinBookmarkRequest, _cancellationToken);

        // Assert
        tooglePinBookmarkResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task ToogleArchiveBookmark_withNonExistent_ReturnsNotFound()
    {
        //Arrange
        using var toogleArchiveBookmarkRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/123/archive");
        toogleArchiveBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var toogleArchiveBookmarkResponse = await _client.SendAsync(toogleArchiveBookmarkRequest, _cancellationToken);

        // Assert
        toogleArchiveBookmarkResponse.Should().Be404NotFound();
    }

    [Fact]
    public async Task ToogleArchiveBookmark_ReturnsOk()
    {
        //Arrange
        using var toogleArchiveBookmarkRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/1/archive");
        toogleArchiveBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var toogleArchiveBookmarkResponse = await _client.SendAsync(toogleArchiveBookmarkRequest, _cancellationToken);

        // Assert
        toogleArchiveBookmarkResponse.Should().Be204NoContent();
    }

    [Fact]
    public async Task ToogleArchiveBookmark_Unauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        using var toogleArchiveBookmarkRequest = new HttpRequestMessage(HttpMethod.Patch, $"{BaseUrl}/1/archive");

        // Act
        using var toogleArchiveBookmarkResponse = await _client.SendAsync(toogleArchiveBookmarkRequest, _cancellationToken);

        // Assert
        toogleArchiveBookmarkResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task DeleteBookmark_withNonExistent_ReturnsNotFound()
    {
        //Arrange
        using var deleteBookmarkRequest = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/123");
        deleteBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var deleteBookmarkResponse = await _client.SendAsync(deleteBookmarkRequest, _cancellationToken);

        // Assert
        deleteBookmarkResponse.Should().Be404NotFound();
    }

    [Fact]
    public async Task DeleteBookmark_withExisting_ReturnsForbid()
    {
        //Arrange
        using var deleteBookmarkRequest = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/2");
        deleteBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var deleteBookmarkResponse = await _client.SendAsync(deleteBookmarkRequest, _cancellationToken);

        // Assert
        deleteBookmarkResponse.Should().Be403Forbidden();
    }

    [Fact]
    public async Task DeleteBookmark_ReturnsOk()
    {
        //Arrange
        using var deleteBookmarkRequest = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/1");
        deleteBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var deleteBookmarkResponse = await _client.SendAsync(deleteBookmarkRequest, _cancellationToken);

        // Assert
        deleteBookmarkResponse.Should().Be204NoContent();
    }

    [Fact]
    public async Task DeleteBookmark_Unauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        using var deleteBookmarkRequest = new HttpRequestMessage(HttpMethod.Delete, $"{BaseUrl}/1");

        // Act
        using var deleteBookmarkResponse = await _client.SendAsync(deleteBookmarkRequest, _cancellationToken);

        // Assert
        deleteBookmarkResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task UpdateBookmark_WithValidData_ReturnsOk()
    {
        //Arrange
        using var updateBookmarkRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/1");
        updateBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var updateBookmarkData = new
        {
            Title = "GitHub",
            Url = "https://github.com",
            Description = "Where the world builds software. Millions of developers and companies build, ship, and maintain their software on GitHub.",
            Tags = new List<string> { "Tools", "Community", "Git" }
        };
        updateBookmarkRequest.Content = JsonContent.Create(updateBookmarkData);

        //Act
        using var updateBookmarkResponse = await _client.SendAsync(updateBookmarkRequest, _cancellationToken);

        // Assert
        updateBookmarkResponse.Should().Be204NoContent();
    }

    [Fact]
    public async Task UpdateBookmark_withNonExistent_ReturnsNotFound()
    {
        //Arrange
        using var updateBookmarkRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/123");
        updateBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var updateBookmarkData = new
        {
            Title = "Test Update",
            Url = "https://example.com",
            Description = "Update Description Test",
            Tags = new[] { "test" }
        };
        updateBookmarkRequest.Content = JsonContent.Create(updateBookmarkData);

        // Act
        using var updateBookmarkResponse = await _client.SendAsync(updateBookmarkRequest, _cancellationToken);

        // Assert
        updateBookmarkResponse.Should().Be404NotFound();
    }

    [Fact]
    public async Task UpdateBookmark_Unauthenticated_ReturnsUnauthorized()
    {
        //Arrange
        using var updateBookmarkRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/1");

        // Act
        using var updateBookmarkResponse = await _client.SendAsync(updateBookmarkRequest, _cancellationToken);

        // Assert
        updateBookmarkResponse.Should().Be401Unauthorized();
    }

    [Fact]
    public async Task UpdateBookmark_WithValidDataAndExisTingTitle_ReturnsConflict()
    {
        //Arrange
        using var updateBookmarkRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/1");
        updateBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var updateBookmarkData = new
        {
            Title = "Stack Overflow",
            Url = "https://github.com",
            Description = "Update Description Test",
            Tags = new List<string> { "Tools", "Community", "Git" }
        };
        updateBookmarkRequest.Content = JsonContent.Create(updateBookmarkData);

        //Act
        using var updateBookmarkResponse = await _client.SendAsync(updateBookmarkRequest, _cancellationToken);

        // Assert
        updateBookmarkResponse.Should().Be409Conflict();
    }

    [Fact]
    public async Task UpdateBookmark_WithValidDataAndExisTingUrl_ReturnsConflict()
    {
        //Arrange
        using var updateBookmarkRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/1");
        updateBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var updateBookmarkData = new
        {
            Title = "GitHub",
            Url = "https://stackoverflow.com",
            Description = "Update Description Test",
            Tags = new List<string> { "Tools", "Community", "Git" }
        };
        updateBookmarkRequest.Content = JsonContent.Create(updateBookmarkData);

        //Act
        using var updateBookmarkResponse = await _client.SendAsync(updateBookmarkRequest, _cancellationToken);

        // Assert
        updateBookmarkResponse.Should().Be409Conflict();
    }

    [Fact]
    public async Task UpdateBookmark_WithInvalidData_ReturnsBadRequest()
    {
        //Arrange
        using var updateBookmarkRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/1");
        updateBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var updateBookmarkData = new
        {
            Title = "T",
            Url = "https:/github.com",
            Description = "Update Description Test",
            Tags = new List<string> { "Tools", "Community", "Git" }
        };
        updateBookmarkRequest.Content = JsonContent.Create(updateBookmarkData);

        //Act
        using var updateBookmarkResponse = await _client.SendAsync(updateBookmarkRequest, _cancellationToken);

        // Assert
        updateBookmarkResponse.Should().Be400BadRequest()
            .And.BeAs(new
            {
                title = "Validation Error",
                status = 400,
                errors = new Dictionary<string, string[]>
                {
                    ["Title"] = ["The length of 'Title' must be at least 3 characters. You entered 1 characters."],
                    ["Url"] = ["The specified condition was not met for 'Url'."],
                }
            });
    }

    [Fact]
    public async Task UpdateBookmark_WithInvalidDataAndSameTag_ReturnsBadRequest()
    {
        //Arrange
        using var updateBookmarkRequest = new HttpRequestMessage(HttpMethod.Put, $"{BaseUrl}/1");
        updateBookmarkRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));
        var updateBookmarkData = new
        {
            Title = "GitHub",
            Url = "https://github.com",
            Description = "Update Description Test",
            Tags = new List<string> { "Tools", "Community", "Git", "Reference", "TOOLS" }
        };
        updateBookmarkRequest.Content = JsonContent.Create(updateBookmarkData);

        //Act
        using var updateBookmarkResponse = await _client.SendAsync(updateBookmarkRequest, _cancellationToken);

        // Assert
        updateBookmarkResponse.Should().Be400BadRequest()
            .And.BeAs(new
            {
                title = "Validation Error",
                status = 400,
                errors = new Dictionary<string, string[]>
                {
                    ["Tags"] = ["Tags must be unique."]
                }
            });
    }

    [Fact]
    public async Task GetAllBookmarkFromSearchTerm_ReturnsOk()
    {
        // Arrange
        const string searchTerm= "github";
        using var getAllBookmarkFromSearchTermRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/search?query={searchTerm}");
        getAllBookmarkFromSearchTermRequest.Headers.Add("Cookie", await _utility.LoginAndGetJwtCookieAsync(_seedUser.Email, _seedUser.Password));

        // Act
        using var getAllBookmarkFromSearchTermResponse = await _client.SendAsync(getAllBookmarkFromSearchTermRequest, _cancellationToken);

        //Assert
        getAllBookmarkFromSearchTermResponse.Should().Be200Ok()
            .And.BeAs(new[]
        {
            new{
                bookmarkId= 1,
                title = "GitHub",
                url = "https://github.com",
                description = "Where the world builds software. Millions of developers and companies build, ship, and maintain their software on GitHub.",
                isPinned     = false,
                isArchived   = true,
                tags = new List<string> { "Community", "Git" },
                visitsCount  = 1,
                lastVisitTime = DateTimeOffset.Parse("2026-02-27T09:00:00Z") as DateTimeOffset?
            }
        }, options => options
            .ExcludingMissingMembers()
            .Excluding(info => info.Path.EndsWith("creationTime"))
        );
    }

    [Fact]
    public async Task GetAllBookmarkFromSearchTerm_Unauthenticated_ReturnsUnauthorized()
    {
        // Arrange
        const string searchTerm= "github";
        using var getAllBookmarkFromSearchTermRequest = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/search?query={searchTerm}");

        // Act
        using var getAllBookmarkFromSearchTermResponse = await _client.SendAsync(getAllBookmarkFromSearchTermRequest, _cancellationToken);

        // Assert
        getAllBookmarkFromSearchTermResponse.Should().Be401Unauthorized();
    }

}