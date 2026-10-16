using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeuroLogicV3.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken] // Ochrona przed CSRF
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToPage("/Index"); // Przekierowanie na stronę główną
        }
    }
}
