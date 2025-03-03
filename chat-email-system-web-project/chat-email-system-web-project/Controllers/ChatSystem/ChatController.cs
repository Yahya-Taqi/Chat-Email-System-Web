using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace chat_email_system_web_project.Controllers.ChatSystem
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            // Retrieve username from session
            var userName = HttpContext.Session.GetString("Username");

            // If no username is found, redirect to login
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Auth");
            }

            // Pass the username to the chat view
            ViewData["UserName"] = userName;
            return View();
        }
    }
}
