using LingoVerse.Models;

namespace LingoVerse.Repositories.Interface
{
    public interface IOfficialPostsRepository : IGenericRepository<OfficialPost>
    {
        Task<(IEnumerable<OfficialPost> Items, int TotalCount)> GetOfficialPostsByPageAsync(
        string searchTerm = null,
        int? categoryId = null,
        int? levelId = null,
        int? languageId = null,
        int pageNumber = 1,
        int pageSize = 10);
    }
}
