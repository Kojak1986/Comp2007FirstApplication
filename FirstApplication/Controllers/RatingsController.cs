using FirstApplication.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

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

        public Rating SetRating(string gameId, decimal rank)
        {
            Rating rating = new Rating();
            rating.Rank = rank;
            rating.GameId = gameId;
            rating.UserId = User.Identity.GetUserId();

            db.Ratings.Add(rating);
            db.SaveChanges();

            rating = db.Ratings
                .Include(x => x.Game)
                .Include(x => x.Game.Ratings)
                .Include(x => x.User)
                .SingleOrDefault(x => x.RatingId == rating.RatingId);
            return (rating);
           // return RedirectToAction("Details", "Games", new { id = gameId });
        }

        public PartialViewResult RatingsControl(string gameId)
        {
            Game model = db.Games.Find(gameId);

            return PartialView("_RatingsControl", model);
        }
    }
}