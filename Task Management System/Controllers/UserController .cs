using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task_Management_System.Models;
using TaskManagement.Buisness.Handlers;
using TaskManagement.Buisness.Interface;
using TaskManagement.Buisness.ViewModels;

namespace Task_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserBuisness userBuisness;
        public UserController(IUserBuisness userBuisness)
        {
            this.userBuisness = userBuisness;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                int status = await userBuisness.ValidateUser(loginModel);
                HttpContext.Session.SetString("userID", status.ToString());
                if (status>0)
                {
                    HttpContext.Session.SetString("userEMail", loginModel.Email);
                    return RedirectToAction("Index", "TaskManagement");
                }
                else
                {
                    ViewBag.Error = "Invalid Username or Password..";
                }
            }

            // If model validation fails, return to the login form with error messages
            return View(loginModel);
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel user)
        {
            if (ModelState.IsValid)
            {
                // Save user to the database or perform other actions
                // For simplicity, let's assume we are just returning a success message
                try
                {
                    var status = await userBuisness.UserRegistration(user);
                    return Content("Registration successful!");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }

            // If model validation fails, return to the registration form with error messages
            return View(user);
        }
        [Route("Logout")]        /// 
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }
    }
}
