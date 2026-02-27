using BookmarkManagerApp.Exceptions;
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services;
using BookmarkManagerApp.Services.Contracts;
using Moq;

namespace BookmarkManagerApp.UnitTests.Services;

public class BookmarkServiceTest
{
    private readonly Mock<IBookmarkRepository> _mockBookmarkRepository;
    private readonly Mock<ITagRepository> _mockTagRepository;
    private readonly Mock<IUserContext> _mockUserContext;
    private readonly BookmarkService _bookmarkService;
    private readonly long _userId = 123L;

    public BookmarkServiceTest()
    {
        _mockBookmarkRepository = new Mock<IBookmarkRepository>();
        _mockTagRepository = new Mock<ITagRepository>();
        _mockUserContext = new Mock<IUserContext>();
        _mockUserContext.Setup(u => u.UserId).Returns(_userId);
        _bookmarkService = new BookmarkService(_mockBookmarkRepository.Object, _mockUserContext.Object, _mockTagRepository.Object);
    }

    [Fact]
    public async Task DeleteAsync_WhenBookmarkDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        const long bookmarkId = 999L;

        _mockBookmarkRepository
            .Setup(r => r.GetByIdAsync(bookmarkId))
            .ReturnsAsync(null as Bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            async () => await _bookmarkService.DeleteAsync(bookmarkId)
        );
        _mockBookmarkRepository.Verify(r => r.GetByIdAsync(bookmarkId), Times.Once);
        _mockBookmarkRepository.Verify(r => r.DeleteAsync(bookmarkId), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenBookmarkExistsButNotArchived_ShouldThrowForbiddenException()
    {
        // Arrange
        const long bookmarkId = 42;
        var bookmark = new Bookmark { BookmarkId = bookmarkId, IsArchived = false };

        _mockBookmarkRepository
            .Setup(r => r.GetByIdAsync(bookmarkId))
            .ReturnsAsync(bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(
            () => _bookmarkService.DeleteAsync(bookmarkId)
        );
        _mockBookmarkRepository.Verify(r => r.GetByIdAsync(bookmarkId), Times.Once);
        _mockBookmarkRepository.Verify(r => r.DeleteAsync(bookmarkId), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenBookmarkExistsAndIsArchived_ShouldDeleteSuccessfully()
    {
        // Arrange
        const long bookmarkId = 42;
        var bookmark = new Bookmark { BookmarkId = bookmarkId, IsArchived = true };

        _mockBookmarkRepository
            .Setup(r => r.GetByIdAsync(bookmarkId))
            .ReturnsAsync(bookmark);
        _mockBookmarkRepository
            .Setup(r => r.DeleteAsync(bookmarkId))
            .Returns(Task.CompletedTask);

        // Act
        await _bookmarkService.DeleteAsync(bookmarkId);

        // Assert
        _mockBookmarkRepository.Verify(r => r.GetByIdAsync(bookmarkId), Times.Once);
        _mockBookmarkRepository.Verify(r => r.DeleteAsync(bookmarkId), Times.Once);
    }

    [Fact]
    public async Task TogglePinAsync_WhenBookmarkDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        const long bookmarkId = 999L;

        _mockBookmarkRepository
            .Setup(r => r.GetByIdAsync(bookmarkId))
            .ReturnsAsync(null as Bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            async () => await _bookmarkService.TogglePinAsync(bookmarkId)
        );
        _mockBookmarkRepository.Verify(r => r.GetByIdAsync(bookmarkId), Times.Once);
        _mockBookmarkRepository.Verify(r => r.TogglePinAsync(bookmarkId), Times.Never);
    }

    [Fact]
    public async Task TogglePinAsync_WhenBookmarkExistsAndIsArchived_ShouldThrowForbiddenException()
    {
        // Arrange
        const long bookmarkId = 42;
        var bookmark = new Bookmark { BookmarkId = bookmarkId, IsArchived = true };

        _mockBookmarkRepository.Setup(r => r.GetByIdAsync(bookmarkId)).ReturnsAsync(bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(
            () => _bookmarkService.TogglePinAsync(bookmarkId)
        );
        _mockBookmarkRepository.Verify(r => r.GetByIdAsync(bookmarkId), Times.Once);
        _mockBookmarkRepository.Verify(r => r.TogglePinAsync(bookmarkId), Times.Never);
    }

    [Fact]
    public async Task TogglePinAsync_WhenBookmarkExistsAndNotArchived_ShouldTogglePinSuccessfully()
    {
        // Arrange
        const long bookmarkId = 42;
        var bookmark = new Bookmark { BookmarkId = bookmarkId, IsArchived = false };

        _mockBookmarkRepository.Setup(r => r.GetByIdAsync(bookmarkId)).ReturnsAsync(bookmark);
        _mockBookmarkRepository.Setup(r => r.TogglePinAsync(bookmarkId)).Returns(Task.CompletedTask);

        // Act & Assert
        await _bookmarkService.TogglePinAsync(bookmarkId);
        _mockBookmarkRepository.Verify(r => r.GetByIdAsync(bookmarkId), Times.Once);
        _mockBookmarkRepository.Verify(r => r.TogglePinAsync(bookmarkId), Times.Once);
    }

    [Fact]
    public async Task ToggleArchiveAsync_WhenBookmarkDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        const long bookmarkId = 999L;

        _mockBookmarkRepository
            .Setup(r => r.ExistsByBookmarkId(bookmarkId))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            async () => await _bookmarkService.ToggleArchiveAsync(bookmarkId)
        );
        _mockBookmarkRepository.Verify(r => r.ExistsByBookmarkId(bookmarkId), Times.Once);
        _mockBookmarkRepository.Verify(r => r.ToggleArchiveAsync(bookmarkId), Times.Never);
    }

    [Fact]
    public async Task ToggleArchiveAsync_WhenBookmarkExists_ShouldToggleArchiveSuccessfully()
    {
        // Arrange
        const long bookmarkId = 42;

        _mockBookmarkRepository.Setup(r => r.ExistsByBookmarkId(bookmarkId)).ReturnsAsync(true);
        _mockBookmarkRepository.Setup(r => r.ToggleArchiveAsync(bookmarkId)).Returns(Task.CompletedTask);

        // Act & Assert
        await _bookmarkService.ToggleArchiveAsync(bookmarkId);
        _mockBookmarkRepository.Verify(r => r.ExistsByBookmarkId(bookmarkId), Times.Once);
        _mockBookmarkRepository.Verify(r => r.ToggleArchiveAsync(bookmarkId), Times.Once);
    }

    [Fact]
    public async Task GetAllByUserIdAndSearchTermAsync_ShouldReturnBookmarksForCurrentUserAndSearchTerm()
    {
        // Arrange
        const string searchTerm = "react";
        var expectedBookmarks = new List<Bookmark>
        {
            new Bookmark { BookmarkId = 1, Title = "React Tutorial", UserId = _userId },
            new Bookmark { BookmarkId = 2, Title = "Advanced React Hooks", UserId = _userId },
            new Bookmark { BookmarkId = 3, Title = "Vue vs React", UserId = _userId }
        };

        _mockBookmarkRepository
            .Setup(r => r.GetAllByUserIdAndSearchTermAsync(_userId, searchTerm))
            .ReturnsAsync(expectedBookmarks);

        // Act
        var result = await _bookmarkService.GetAllByUserIdAndSearchTermAsync(searchTerm);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBookmarks.Count, result.Count());
        Assert.Equal(expectedBookmarks, result);
        _mockBookmarkRepository.Verify(r => r.GetAllByUserIdAndSearchTermAsync(_userId, searchTerm), Times.Once());
    }

    [Fact]
    public async Task GetAllByUserIdAsync_ShouldReturnBookmarksForCurrentUser()
    {
        // Arrange
        var expectedBookmarks = new List<Bookmark>
        {
            new Bookmark { BookmarkId = 1, Title = "React Tutorial", UserId = _userId },
            new Bookmark { BookmarkId = 2, Title = "Advanced React Hooks", UserId = _userId },
            new Bookmark { BookmarkId = 3, Title = "Vue vs React", UserId = _userId }
        };

        _mockBookmarkRepository
            .Setup(r => r.GetAllByUserIdAsync(_userId))
            .ReturnsAsync(expectedBookmarks);

        // Act
        var result = await _bookmarkService.GetAllByUserIdAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedBookmarks.Count, result.Count());
        Assert.Equal(expectedBookmarks, result);
        _mockBookmarkRepository.Verify(r => r.GetAllByUserIdAsync(_userId), Times.Once());
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookmarkDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        const long bookmarkId = 999L;

        _mockBookmarkRepository
            .Setup(r => r.GetByIdAsync(bookmarkId))
            .ReturnsAsync(null as Bookmark);

        //Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            async () => await _bookmarkService.GetByIdAsync(bookmarkId)
        );
        _mockBookmarkRepository.Verify(r => r.GetByIdAsync(bookmarkId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookmarkExists_ShouldReturnBookmark()
    {
        // Arrange
        const long bookmarkId = 42;
        var expected = new Bookmark { BookmarkId = bookmarkId, Title = "Test" };

        _mockBookmarkRepository.Setup(r => r.GetByIdAsync(bookmarkId)).ReturnsAsync(expected);

        // Act
        var result = await _bookmarkService.GetByIdAsync(bookmarkId);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(expected, result);
        _mockBookmarkRepository.Verify(r => r.GetByIdAsync(bookmarkId), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithNonExistentBookmark_ThrowsNotFoundException()
    {
        // Arrange
        _mockBookmarkRepository
            .Setup(x => x.GetByIdForUpdateAsync(It.IsAny<long>()))
            .ReturnsAsync(null as Bookmark);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _bookmarkService.UpdateAsync(It.IsAny<long>(), It.IsAny<CreateOrUpdateBookmarkCommand>()));
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateTitle_ThrowsConflictException()
    {
        // Arrange
        const long bookmarkId = 1L;
        var existingBookmark = new Bookmark
        { BookmarkId = bookmarkId, Title = "Old Title", Url = "https://url.com", Tags = new List<Tag>() };
        var command = new CreateOrUpdateBookmarkCommand("New Title", "https://url.com", "Description", []);

        _mockBookmarkRepository
            .Setup(x => x.GetByIdForUpdateAsync(bookmarkId))
            .ReturnsAsync(existingBookmark);
        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndTitle(_userId, command.Title))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _bookmarkService.UpdateAsync(bookmarkId, command));
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateUrl_ThrowsConflictException()
    {
        // Arrange
        const long bookmarkId = 1L;
        var existingBookmark = new Bookmark
            { BookmarkId = bookmarkId, Title = "Title", Url = "https://old.com", Tags = new List<Tag>() };
        var command =
            new CreateOrUpdateBookmarkCommand("Title", "https://new.com", "Description", []);
    
        _mockBookmarkRepository
            .Setup(x => x.GetByIdForUpdateAsync(bookmarkId))
            .ReturnsAsync(existingBookmark);
        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndTitle(_userId, command.Title))
            .ReturnsAsync(false);
        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndUrl(_userId, command.Url))
            .ReturnsAsync(true);
    
        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _bookmarkService.UpdateAsync(bookmarkId, command));
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_UpdatesBookmark()
    {
        // Arrange
        const long bookmarkId = 1L;
        var existingBookmark = new Bookmark
            { BookmarkId = bookmarkId, Title = "Old Title", Url = "https://old.com", Tags = new List<Tag>() };
        var command =
            new CreateOrUpdateBookmarkCommand("New Title", "https://new.com", "Description", ["tag1", "tag2"]);
        var existingTag = new Tag { Name = "tag1" };

        _mockBookmarkRepository
            .Setup(x => x.GetByIdForUpdateAsync(bookmarkId))
            .ReturnsAsync(existingBookmark);
        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndTitle(_userId, command.Title))
            .ReturnsAsync(false);
        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndUrl(_userId, command.Url))
            .ReturnsAsync(false);
        _mockTagRepository
            .Setup(x => x.GetByNames(command.TagNames))
            .ReturnsAsync([existingTag]);

        // Act
        await _bookmarkService.UpdateAsync(bookmarkId, command);

        // Assert
        Assert.Equal("New Title", existingBookmark.Title);
        Assert.Equal("https://new.com", existingBookmark.Url);
        Assert.Equal("Description", existingBookmark.Description);
        Assert.Equal(2, existingBookmark.Tags.Count);
        _mockBookmarkRepository.Verify(x => x.UpdateAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithDuplicateTitleOrUrl_ThrowsConflictException()
    {
        // Arrange
        var command = new CreateOrUpdateBookmarkCommand("Title", "https://url.com", "Description", []);
        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndTitleOrUrl(_userId, command.Title, command.Url))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() => _bookmarkService.CreateAsync(command));
    }

    [Fact]
    public async Task CreateAsync_WithValidDataAndExistingTags_CreatesBookmarkWithTags()
    {
        // Arrange
        var command = new CreateOrUpdateBookmarkCommand("Title", "https://url.com", "Description", ["tag1", "tag2"]);
        var existingTag = new Tag { Name = "tag1" };
        var createdBookmark = new Bookmark
        {
            BookmarkId = 1, Title = command.Title, Url = command.Url, Description = command.Description,
            Tags = new List<Tag> { new() { Name = "tag1" }, new() { Name = "tag2" } }
        };

        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndTitleOrUrl(_userId, command.Title, command.Url))
            .ReturnsAsync(false);
        _mockTagRepository
            .Setup(x => x.GetByNames(command.TagNames))
            .ReturnsAsync([existingTag]);
        _mockBookmarkRepository
            .Setup(x => x.CreateAsync(It.IsAny<Bookmark>()))
            .ReturnsAsync(createdBookmark);

        // Act
        var result = await _bookmarkService.CreateAsync(command);

        // Assert
        Assert.Equal(createdBookmark, result);
        _mockBookmarkRepository.Verify(x => x.CreateAsync(It.Is<Bookmark>(b => b.Tags.Count == 2)), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidDataAndNewTags_CreatesBookmarkWithNewTags()
    {
        // Arrange
        var command =
            new CreateOrUpdateBookmarkCommand("Title", "https://url.com", "Description", ["newTag1", "newTag2"]);
        var createdBookmark = new Bookmark
        {
            BookmarkId = 1, Title = command.Title, Url = command.Url, Description = command.Description,
            Tags = new List<Tag> { new() { Name = "newTag1" }, new() { Name = "newTag2" } }
        };

        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndTitleOrUrl(_userId, command.Title, command.Url))
            .ReturnsAsync(false);
        _mockTagRepository
            .Setup(x => x.GetByNames(command.TagNames))
            .ReturnsAsync([]);
        _mockBookmarkRepository
            .Setup(x => x.CreateAsync(It.IsAny<Bookmark>()))
            .ReturnsAsync(createdBookmark);

        // Act
        var result = await _bookmarkService.CreateAsync(command);

        // Assert
        Assert.Equal(createdBookmark, result);
        _mockBookmarkRepository.Verify(x => x.CreateAsync(It.Is<Bookmark>(b =>
            b.Tags.Count == 2 &&
            b.Tags.All(t => command.TagNames.AsEnumerable().Contains(t.Name))
        )), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidDataAndNoTags_CreatesBookmark()
    {
        // Arrange
        var command = new CreateOrUpdateBookmarkCommand("Title", "https://url.com", "Description", []);
        var createdBookmark = new Bookmark
            { BookmarkId = 1, Title = command.Title, Url = command.Url, Description = command.Description };

        _mockBookmarkRepository
            .Setup(x => x.ExistsByUserIdAndTitleOrUrl(_userId, command.Title, command.Url))
            .ReturnsAsync(false);
        _mockBookmarkRepository
            .Setup(x => x.CreateAsync(It.IsAny<Bookmark>()))
            .ReturnsAsync(createdBookmark);

        // Act
        var result = await _bookmarkService.CreateAsync(command);

        // Assert
        Assert.Equal(createdBookmark, result);
        _mockBookmarkRepository.Verify(x => x.CreateAsync(It.Is<Bookmark>(b =>
            b.UserId == _userId &&
            b.Title == command.Title &&
            b.Url == command.Url &&
            b.Description == command.Description &&
            b.Tags.Count == 0
        )), Times.Once);
    }
    
}