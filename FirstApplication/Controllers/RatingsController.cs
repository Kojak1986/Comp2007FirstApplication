using FirstApplication.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstApplication.Controllers
{
    public class RatingsController : Controller
    {
        DataContext db = new DataContext();
        // GET: Ratings
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SetRating(string gameId, decimal rank)
        {
            Rating rating = new Rating();
            rating.Rank = rank;
            rating.GameId = gameId;
            rating.UserId = User.Identity.GetUserId();

            db.Ratings.Add(rating);
            db.SaveChanges();

            return RedirectToAction("Details", "Games", new { id = gameId });
        }
    }
}