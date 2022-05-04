using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MyContext db;

        public HomeController(MyContext conext, ILogger<HomeController> logger)
        {
            _logger = logger;
            db = conext;
        }
        public int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }
        // REGISTRATION ============================================================================
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        // =====(If success, Re:Dash)======
        [HttpPost("create")]
        public IActionResult Create(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is Already in use.");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

                db.Users.Add(newUser);
                db.SaveChanges();

                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                HttpContext.Session.SetString("Username", newUser.FirstName);
                return RedirectToAction("Dashboard", "Wedding");
            }
            return View("Index");
        }
        // LOGIN =================================================================================
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
        // =====(If success, Re:Dash)======
        [HttpPost("login/request")]
        public IActionResult Logged(LoginUser user)
        {
            if (ModelState.IsValid)
            {
                User userInDB = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (userInDB == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, userInDB.Password, user.Password);

                if (result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                HttpContext.Session.SetInt32("UserId", userInDB.UserId);
                HttpContext.Session.SetString("Username", userInDB.FirstName);
                return RedirectToAction("Dashboard", "Wedding");
            }
            return View("Login");
        }
        // LOGOUT =================================================================================
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login");
        }
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            if (uid == null)
            {
                return RedirectToAction("Login");
            }
            User loggedUser = db.Users.FirstOrDefault(u => u.UserId == uid);

            // HttpContext.Session.SetInt32("UserId", user.UserId);
            // int? UserIdSesssion = HttpContext.Session.GetInt32("UserId");
            // var User = db.Users.FirstOrDefault(u => u.UserId == UserIdSesssion);

            return View(loggedUser);
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
