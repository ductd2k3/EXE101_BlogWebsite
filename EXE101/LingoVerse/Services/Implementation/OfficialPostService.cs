using LingoVerse.Models;
using LingoVerse.Repositories.Interface;
using LingoVerse.Services.Interface;

namespace LingoVerse.Services.Implementation
{
    public class OfficialPostService : GenericService<OfficialPost>, IOfficialPostService
    {
        private readonly IOfficialPostsRepository _postsRepository;
        public OfficialPostService(IOfficialPostsRepository repository) : base(repository)
        {
            _postsRepository = repository;
        }

        public async Task<(IEnumerable<OfficialPost> Items, int TotalCount)> GetOfficialPostsByPageAsync(string searchTerm = null, int? categoryId = null, int? levelId = null, int? languageId = null, int pageNumber = 1, int pageSize = 10)
        {
            return await _postsRepository.GetOfficialPostsByPageAsync(searchTerm, categoryId, levelId, languageId, pageNumber, pageSize);
        }
    }
}
