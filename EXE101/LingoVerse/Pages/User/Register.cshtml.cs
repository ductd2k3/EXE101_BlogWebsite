using LingoVerse.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LingoVerse.Pages.User
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userService;
        public RegisterModel(IUserService userService)
        {
            _userService = userService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(IFormCollection form)
        {
            Models.User User = new Models.User
            {
                Username = form["userName"],
                PasswordHash = form["password"],
                Email = form["email"],
                RoleId = 2
            };

            bool isSuccess = await _userService.RegisterAsync(User);

            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Đăng ký thành công";
                return RedirectToPage("/User/Login");
            }

            TempData["ErrorMessage"] = "Tài khoản đã tồn tại!";
            return Page();
        }
    }
}
