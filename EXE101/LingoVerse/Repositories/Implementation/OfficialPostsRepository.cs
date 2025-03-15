using LingoVerse.Models;
using LingoVerse.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LingoVerse.Repositories.Implementation
{
    public class OfficialPostsRepository : GenericRepository<OfficialPost>, IOfficialPostsRepository
    {
        private readonly LinguaVerseContext _context;
        public OfficialPostsRepository(LinguaVerseContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<OfficialPost> Items, int TotalCount)> GetOfficialPostsByPageAsync(string searchTerm = null, int? categoryId = null, int? levelId = null, int? languageId = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.OfficialPosts
                .Include(p => p.Category)
                .AsQueryable();

            // Tìm kiếm
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(searchTerm));
            }

            // Lọc
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (levelId.HasValue)
                query = query.Where(p => p.LevelId == levelId.Value);

            if (languageId.HasValue)
                query = query.Where(p => p.LanguageId == languageId.Value);

            // Tổng số bản ghi
            int totalCount = await query.CountAsync();

            // Phân trang
            var items = await query
                .OrderByDescending(post => post.PostId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
