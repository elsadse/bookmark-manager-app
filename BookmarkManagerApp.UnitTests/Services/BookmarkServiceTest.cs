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

}