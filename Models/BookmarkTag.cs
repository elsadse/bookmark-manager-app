namespace bookmark_manager_app.Models;

public sealed class BookmarkTag
{
    public int BookmarkId { get; private set; }
    public int TagId { get; private set; }
    public Bookmark? Bookmark { get; private set; }
    public Tag? Tag { get; private set; }

    private BookmarkTag() { }

    private BookmarkTag(int bookmarkId, int tagId)
    {
        BookmarkId = bookmarkId;
        TagId      = tagId;
    }

    public static BookmarkTag Create(int bookmarkId, int tagId)
    {
        if (bookmarkId <= 0)
            throw new ArgumentException("BookmarkId must be positive", nameof(bookmarkId));
        if (tagId <= 0)
            throw new ArgumentException("TagId must be positive", nameof(tagId));
        return new BookmarkTag(bookmarkId, tagId);
    }
}