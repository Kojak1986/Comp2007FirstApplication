using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstApplication.Models;

namespace FirstApplication.Controllers
{
    public class GamesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Games
        public ActionResult Index()
        {
            // var games = db.Games.Include(g => g.GameGenres);
            // return View(games.ToList());
            //return View(db.Games.ToList());

            var games = db.Games.AsQueryable();

            //order by name
            games = games.OrderBy(x => x.Name).AsQueryable();

            return View(games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,IsMultiplayer")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.GameId = Guid.NewGuid().ToString();
                game.CreateDate = DateTime.Now;
                game.EditDate = game.CreateDate;

                db.Games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(game);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Game game = db.Games.Find(id);
            // Game game = db.Games.SingleOrDefault(x => x.GameId == id);


            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameId,Name,IsMultiplayer")] Game game)
        {
            if (ModelState.IsValid)
            {
                Game tmpgame = db.Games.Find(game.GameId);
                if (tmpgame != null)
                {
                    tmpgame.Name = game.Name;
                    tmpgame.EditDate = DateTime.Now;
                    tmpgame.IsMultiplayer = game.IsMultiplayer;


                    db.Entry(tmpgame).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(game);
        }

        // GET: Games/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Game game = db.Games.Find(id);

            //delete game with genres attached
            foreach (var genredata in game.Genres.ToList())
            {
                db.GameGenres.Remove(genredata);
            }

            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
