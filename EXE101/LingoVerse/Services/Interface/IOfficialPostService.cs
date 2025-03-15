using LingoVerse.Models;

namespace LingoVerse.Services.Interface
{
    public interface IOfficialPostService : IGenericService<OfficialPost>
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
