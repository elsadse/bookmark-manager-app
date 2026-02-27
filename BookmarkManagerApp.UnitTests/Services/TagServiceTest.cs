using BookmarkManagerApp.Models;
using BookmarkManagerApp.Repositories.Contracts;
using BookmarkManagerApp.Services;
using BookmarkManagerApp.Services.Contracts;
using Moq;

namespace BookmarkManagerApp.UnitTests.Services;

public class TagServiceTest
{
    private readonly Mock<ITagRepository> _mockTagRepository;
    private readonly Mock<IUserContext> _mockUserContext;
    private readonly TagService _tagService;
    private readonly long _userId = 123L;

    public TagServiceTest()
    {
        _mockTagRepository = new Mock<ITagRepository>();
        _mockUserContext = new Mock<IUserContext>();
        _mockUserContext.Setup(u => u.UserId).Returns(_userId);
        _tagService = new TagService( _mockTagRepository.Object, _mockUserContext.Object);
    }

    [Fact]
    public async Task GetTagsAsync_ShouldReturnAllTags()
    {
        // Arrange
        var expectedTags = new List<Tag>
        {
            new() { TagId = 1, Name = "Technology" },
            new() { TagId = 2, Name = "Programming" },
            new() { TagId = 3, Name = "CSharp" }
        };

        _mockTagRepository.Setup(repo => repo.GetTagAllForUserAsync(_userId))
            .ReturnsAsync(expectedTags);


        // Act
        var result = (await _tagService.GetTagsAsync()).ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTags.Count, result.Count);
        Assert.Equal(expectedTags, result);
        
        _mockTagRepository.Verify(repo => repo.GetTagAllForUserAsync(_userId), Times.Once);
    }
}