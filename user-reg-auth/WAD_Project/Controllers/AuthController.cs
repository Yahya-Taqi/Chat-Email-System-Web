using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WAD_Project.Models;
using WAD_Project.Repositories;

namespace WAD_Project.Controllers
{
    public class AuthController : Controller
    {
        private UserRepository userRepository = new UserRepository();

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User user, string password, string confirmPassword)
        {
            if (!user.Email.EndsWith("@muufa.com"))
            {
                ViewBag.Error = "Only emails with the domain '@muufa.com' are allowed.";
                return View();
            }
            if (userRepository.GetUserByEmail(user.Email) != null)
            {
                ViewBag.Error = "Email already exists.";
                return View();
            }
            if (password != confirmPassword)
            {
                ViewBag.Error = "Password and Confirm Password do not match.";
                return View();
            }
            user.Password = password; 
            userRepository.RegisterUser(user);
            ViewBag.Success = "Account created successfully! You can now log in.";
            return View();
        }


        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = userRepository.GetUserByEmail(email);
            if (user != null && user.Password == password)
            {
                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }
    }

}