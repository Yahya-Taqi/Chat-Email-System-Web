using Microsoft.AspNetCore.Mvc;

namespace chat_email_system_web_project.Controllers.ChatSystem
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }
    }
}
