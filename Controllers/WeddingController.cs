using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller
    {
        private readonly ILogger<WeddingController> _logger;
        private MyContext db;
        public WeddingController(MyContext conext, ILogger<WeddingController> logger)
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
        // Wedding Routes================================================================================
            // Rendering the New view (1)
        [HttpGet("Wedding")]
        public IActionResult New()
        {
            if (uid == null)
            {
                return RedirectToAction("Login", "Home");
            }
            
            return View();
        }
        //Create a post===VVV=== (2)
        [HttpPost("Wedding/create")]
        public IActionResult NewPost(Wedding newWedding)
        {
            if (uid == null)
            {
                return RedirectToAction("Login", "Home");
            }
                // New Post - Do not forget (int)uid====
            if (ModelState.IsValid)
            {
                newWedding.UserId = (int)uid;
                db.Weddings.Add(newWedding);
                db.SaveChanges();
                return RedirectToAction("Details", new {weddingId = newWedding.WeddingId});
            }
            return View("New");
        }
            //Detailed ONE post===VVV=== (3)
        [HttpGet("Wedding/{weddingId}")]
        public IActionResult Details(int weddingId)
        {
            if (uid == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Wedding OneWed = db.Weddings
            .Include(w => w.AttendWed)
            .ThenInclude(aw => aw.User)
            .FirstOrDefault(w => w.WeddingId == weddingId);

            if (OneWed == null)
            {
                return RedirectToAction("Dashboard");
            }

            return View("One", OneWed);
        }
        // Dashboard Routes=================================================================================

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            if (uid == null)
            {
                return RedirectToAction("Login", "Home");
            }
            // List of all posts/weddings posted. Also to find Count of (Likes/Attendance).
            List<Wedding> AllWeds = db.Weddings
            .Include(w => w.AttendWed)
            .ThenInclude(aw => aw.User)
            .ToList();

            return View("Dashboard", AllWeds);
        }
        // User Actions and Instances of a post (POST REQs)==================================================
            // like posts POST ======VVV======= (Re:Dash)
        [HttpPost("Wedding/{weddingId}/like")]
        public IActionResult Like(int weddingId)
        {
            if (uid == null)
            {
                return RedirectToAction("Login", "Home");
            }
            // Finding EXISTING Likes for the Like/Unlike instance --VVV--
            Attendance existingLike = db.Attendees
            .FirstOrDefault(att => att.UserId == uid && att.WeddingId == weddingId);

                    // ====== LIKE
            if (existingLike == null)
            {
                Attendance newLike = new Attendance()
                {
                    UserId = (int)uid,
                    WeddingId = weddingId
                };
                db.Attendees.Add(newLike);
            }
                    // ====== UNLIKE
            else
            {
                db.Attendees.Remove(existingLike);
            }
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

            // DELETE posts POST ======VVV====== (Re:Dash)
        [HttpPost("Wedding/{weddingId}/delete")]
        public IActionResult Delete(int weddingId)
        {
            if (uid == null)
            {
                return RedirectToAction("Login", "Home");
            }
            Wedding post = db.Weddings
            .FirstOrDefault(w => w.WeddingId == weddingId);
            
            if(post == null || post.UserId != uid)
            {
                return RedirectToAction("Dashboard");
            }
            db.Weddings.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}