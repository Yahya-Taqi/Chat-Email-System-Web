using Microsoft.AspNetCore.Mvc;

namespace chat_email_system_web_project.Controllers.EmailSystem
{
    public class EmailController : Controller
    {
        public IActionResult Inbox()
        {
            return View();
        }

        public IActionResult Compose()
        {
            return View();
        }
    }
}
