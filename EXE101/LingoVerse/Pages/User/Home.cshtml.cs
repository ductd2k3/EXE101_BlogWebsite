using LingoVerse.Models;
using LingoVerse.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LingoVerse.Pages.User
{
    public class HomeModel : PageModel
    {
        private readonly IOfficialPostService _postService;
        private readonly IGenericService<Language> _languageService;
        private readonly IGenericService<Level> _levelService;
        private readonly IGenericService<Category> _categoryService;
        public HomeModel(IOfficialPostService postService
            , IGenericService<Language> languageService
            , IGenericService<Level> levelService
            , IGenericService<Category> categoryService)
        {
            _postService = postService;
            _languageService = languageService;
            _levelService = levelService;
            _categoryService = categoryService;
        }
        public IEnumerable<OfficialPost> Posts { get; private set; } = Enumerable.Empty<OfficialPost>();
        public IEnumerable<Language> Languages { get; private set; } = Enumerable.Empty<Language>();
        public IEnumerable<Category> Categories { get; private set; } = Enumerable.Empty<Category>();
        public IEnumerable<Level> Levels { get; set; } = Enumerable.Empty<Level>();
        public int TotalCount { get; set; }
        public int PageSize { get; } = 11;
        public int CurrentPage { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? LevelId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? LanguageId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }
        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = Math.Max(pageNumber, 1);
            Categories = await _categoryService.GetAllAsync();
            Languages = await _languageService.GetAllAsync();
            Levels = await _levelService.GetAllAsync();
            var (posts, totalItems) = await _postService.GetOfficialPostsByPageAsync(SearchTerm,CategoryId, LevelId, LanguageId, pageNumber, PageSize);
            Posts = posts ?? Enumerable.Empty<OfficialPost>(); ;
            TotalCount = totalItems;

            return Page();
        }
    }
}
