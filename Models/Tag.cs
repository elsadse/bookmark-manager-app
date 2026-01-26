using bookmark_manager_app.DTOs;

namespace bookmark_manager_app.Models;

public sealed class Tag
{
    public int TagId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    private readonly List<BookmarkTag> _bookmarkTags = new();
    public IReadOnlyCollection<BookmarkTag> BookmarkTags => _bookmarkTags.AsReadOnly();

    public Tag(string name)
    {
        if (name.Length > 25)
            throw new ArgumentException("length of name must be a under 25 character", nameof(name));
        Name = name;

    }
}
