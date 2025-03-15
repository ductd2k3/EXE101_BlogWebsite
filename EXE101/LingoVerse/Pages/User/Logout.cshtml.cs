using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LingoVerse.Pages.User
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Xóa Session
            HttpContext.Session.Remove("UserSession");
            // Hoặc xóa toàn bộ Session: HttpContext.Session.Clear();

            // Redirect về trang chủ hoặc login
            return RedirectToPage("/User/Login");
        }
    }
}
