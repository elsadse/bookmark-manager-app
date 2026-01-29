using bookmark_manager_app.Models;

namespace bookmark_manager_app.Interfaces;

public interface IVisitRepository
{
    Task<Visit> CreateAsync(Visit visit);
    Task<int> GetVisitCountByBookmarkIdAsync(int bookmarkId);
    Task<DateTime?> GetLastVisitDateByBookmarkIdAsync(int bookmarkId);
    Task<Visit?> GetByIdAsync(int visitId);
    Task<IEnumerable<Visit>> GetByBookmarkIdAsync(int bookmarkId);
    Task CreateRangeAsync(IEnumerable<Visit> visits);
    Task DeleteAsync(int visitId);
    Task DeleteByBookmarkIdAsync(int bookmarkId);

}