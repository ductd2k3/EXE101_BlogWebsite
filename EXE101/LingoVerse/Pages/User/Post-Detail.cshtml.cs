using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using LingoVerse.Services.Interface;
using System.Threading.Tasks;

namespace LingoVerse.Pages.User
{
    public class Post_DetailModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserPremiumSubscriptionsService _userPremiumSubscriptionsService;

        public bool IsPremium { get; set; }
        public Models.User User { get; set; } // Đổi từ string CurrentUser sang đối tượng User

        public Post_DetailModel(IHttpContextAccessor httpContextAccessor, IUserPremiumSubscriptionsService userPremiumSubscriptionsService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userPremiumSubscriptionsService = userPremiumSubscriptionsService;
        }

        public async Task OnGetAsync()
        {
            // Lấy thông tin user từ session (giả định session lưu JSON hoặc chuỗi chứa thông tin user)
            var userJson = _httpContextAccessor.HttpContext?.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(userJson))
            {
                User = new Models.User { UserId = 0, Username = "Guest" }; // User mặc định nếu không có session
            }
            else
            {
                // Giả định UserSession là JSON, deserialize thành đối tượng User
                User = System.Text.Json.JsonSerializer.Deserialize<Models.User>(userJson);
            }

            // Lấy thông tin subscription dựa trên User.Id
            var subscription = await _userPremiumSubscriptionsService.GetByUserId(User.UserId);
            IsPremium = subscription != null && subscription.EndDate > DateTime.Now;
        }
    }

}