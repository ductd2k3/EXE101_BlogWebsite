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
            if(userLogin == null)
            {
                TempData["ErrorMessage"] = "Tài khoản không tồn tại!";
                return Page();
            }
            HttpContext.Session.SetString("UserSession",JsonSerializer.Serialize(userLogin));
            //HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(user));
            return RedirectToPage("/User/Home");
        }
    }
}
