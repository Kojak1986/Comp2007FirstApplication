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
            var games = db.Games;
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
            Game model = new Game();
            model.Name = string.Format("Game - {0}", DateTime.Now.Ticks);

            ViewBag.Genres = new MultiSelectList(db.Genres.ToList(), "GenreId", "Name", model.Genres.Select(x=>x.GenreId).ToArray());

            return View(model);
        }

        // POST: Games/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,IsMultiplayer,GenreIds")] Game model, string[] GenreIds)
        {
            if (ModelState.IsValid)
               {
                Game checkModel = db.Games.SingleOrDefault(x => x.Name == model.Name && x.IsMultiplayer == model.IsMultiplayer);

                if (checkModel == null)
                    {
                        //model.GameId = Guid.NewGuid().ToString();
                        //model.CreateDate = DateTime.Now;
                       // model.EditDate = model.CreateDate;
                        db.Games.Add(model);
                        db.SaveChanges();

                    if (GenreIds != null)
                    {
                        foreach (string genreid in GenreIds)
                        {
                            GameGenre gamegenre = new GameGenre();

                            //gamegenre.GameGenreId = Guid.NewGuid().ToString();
                            //gamegenre.CreateDate = DateTime.Now;
                           // gamegenre.EditDate = gamegenre.CreateDate;

                            gamegenre.GameId = model.GameId;
                            gamegenre.GenreId = genreid;
                            db.GameGenres.Add(gamegenre);
                        }

                        db.Entry(model).State = EntityState.Modified;

                        db.SaveChanges();
                    }
                        

                        return RedirectToAction("Index");
                    }
                else
                {
                    ModelState.AddModelError("", "Duplicate Game Found");
                }
            }
                
            ViewBag.Genres = new MultiSelectList(db.Genres.ToList(), "GenreId", "Name", GenreIds);

            return View(model);
        }

        // GET: Games/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game model = db.Games.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            ViewBag.Genres = new MultiSelectList(db.Genres.ToList(), "GenreId", "Name", model.Genres.Select(x => x.GenreId).ToArray());

            return View(model);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameId,Name,IsMultiplayer,GenreIds")] Game model, string[] GenreIds )
        {
            if (ModelState.IsValid)
            {
                Game tmpModel = db.Games.Find(model.GameId);
                if (tmpModel != null)
                {
                    Game checkModel = db.Games.SingleOrDefault(
                                        x => x.Name == model.Name && 
                                        x.IsMultiplayer == model.IsMultiplayer && 
                                        x.GameId != model.GameId);

                    if (checkModel == null)
                    {
                        tmpModel.Name = model.Name;
                        tmpModel.EditDate = DateTime.Now;
                        tmpModel.IsMultiplayer = model.IsMultiplayer;


                        db.Entry(tmpModel).State = EntityState.Modified;

                        //Items to remove
                        var removeItems = tmpModel.Genres.Where(x => !GenreIds.Contains(x.GenreId)).ToList();

                        foreach (var removeItem in removeItems)
                        {
                            db.Entry(removeItem).State = EntityState.Deleted;
                        }

                        if (GenreIds != null)
                        {
                            var addItems = GenreIds.Where(x => !tmpModel.Genres.Select(y => y.GenreId).Contains(x));
                            //Items to add
                            foreach (string addItem in addItems)
                            {
                                GameGenre gamegenre = new GameGenre();
                                gamegenre.GameGenreId = Guid.NewGuid().ToString();
                                gamegenre.CreateDate = DateTime.Now;
                                gamegenre.EditDate = gamegenre.CreateDate;

                                gamegenre.GameId = tmpModel.GameId;
                                gamegenre.GenreId = addItem;

                                db.GameGenres.Add(gamegenre);
                            }
                        }

                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Duplicated game detected. ");
                    }
                }
            }

            ViewBag.Genres = new MultiSelectList(db.Genres.ToList(), "GenreId", "Name", GenreIds);

            return View(model);
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
            Game model = db.Games.Find(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            foreach (var item in model.Genres.ToList())
            {
                db.GameGenres.Remove(item);
            }

            db.Games.Remove(model);

            var deleted = db.ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted);
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
