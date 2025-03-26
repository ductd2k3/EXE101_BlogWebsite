using LingoVerse.Models;
using LingoVerse.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace LingoVerse.Pages.User
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userService;
        public LoginModel(IUserService userService)
        {
            _userService = userService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection form)
        {
            Models.User user = new Models.User
            {
                Email = form["email"],
                PasswordHash = form["password"]
            };

            var userLogin = await _userService.LoginAsync(user);
            if (userLogin == null)
            {
                TempData["ErrorMessage"] = "Tài khoản không tồn tại!";
                return Page();
            }

            // Lưu thông tin người dùng vào session
            HttpContext.Session.SetString("UserSession", JsonSerializer.Serialize(userLogin));

            // Kiểm tra RoleId để chuyển hướng
            if (userLogin.RoleId == 1)
            {
                return RedirectToPage("/Manager/Admin-Dashboard"); // Trang admin
            }
            else if (userLogin.RoleId == 2)
            {
                return RedirectToPage("/User/Home"); // Trang home của user
            }
            else
            {
                TempData["ErrorMessage"] = "Vai trò không hợp lệ!";
                return Page(); // Trở lại trang login nếu RoleId không xác định
            }
        }
    }
}
