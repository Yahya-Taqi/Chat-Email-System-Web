using Microsoft.AspNetCore.Mvc;

namespace chat_email_system_web_project.Controllers.SignupLogin
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}
