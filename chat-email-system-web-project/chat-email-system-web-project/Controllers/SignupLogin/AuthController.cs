using Microsoft.AspNetCore.Mvc;
using chat_email_system_web_project.Models.SignupLogin;
using chat_email_system_web_project.Repositories;
using Microsoft.AspNetCore.Http;
using System;

namespace chat_email_system_web_project.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserRepository _userRepository;

        public AuthController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);

            if (user != null && user.Password == password)
            {
                // ✅ Store user session
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);

                // ✅ Redirect to ChatController → Chat action
                return RedirectToAction("Chat", "Chat");
            }

            ViewData["Error"] = "Invalid email or password.";
            return View();
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewData["Error"] = "Passwords do not match!";
                return View();
            }

            // Ensure user object has correct password
            user.Password = password;

            if (_userRepository.GetUserByEmail(user.Email) != null)
            {
                ViewData["Error"] = "Email already exists.";
                return View();
            }

            bool isRegistered = _userRepository.RegisterUser(user);

            if (isRegistered)
            {
                // Redirect to Login after successful signup
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ViewData["Error"] = "Database error. Please try again.";
                return View();
            }
        }
    }
}
