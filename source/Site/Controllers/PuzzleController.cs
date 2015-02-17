using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;
using Toppuzzle.Site.Models;

namespace Toppuzzle.Site.Controllers {
    public class PuzzleController : BaseController {
        // GET: Puzzle
        //public ActionResult Editor() {
        //    return View();
        //}

        public ActionResult Editor(string pictureId, int complexity) {
            return View(new EditorViewModel{Complexity = complexity, PictureId = pictureId});
        }

        public ActionResult Temp() {
            return RedirectToAction("GetPicture", new { pictureId = "15611247863", complexity = 1 });
        }

        public ActionResult GetPicture(string pictureId, int complexity) {
            var picture = ApplicationFacade.Instance.PictureManager.GetPictureByPictureId(pictureId);
            var source = "~/Content/Puzzles/" + picture.Picture;
            var image = new Bitmap(Server.MapPath(source));
            var cuttedImages = ApplicationFacade.Instance.PictureManager.CutImage(image, complexity);
            var randomList = ApplicationFacade.Instance.PictureManager.GetRandomList(complexity);
            return View(new PictureViewModel
            {
                Complexity = complexity,
                Height = image.Height,
                Width = image.Width,
                Parts = cuttedImages,
                Source = source,
                RandomList = randomList
            });
        }

        [HttpPost]
        public void GetResult(string time, string complexity, string puzzleId) {
            var user = ApplicationFacade.Instance.GetCurrentUser();
            if (user == null) return;
            var score = new Score {
                Complexity = int.Parse(complexity),
                Date = DateTime.Today,
                PictureId = puzzleId,
                Time = int.Parse(time),
                UserId = user.Id
            };
            user.Rating += score.Complexity * 20;
            ApplicationFacade.Instance.ScoreManager.InsertNewScore(score);
            ApplicationFacade.Instance.UserManager.UpdateUser(user);
            ApplicationFacade.Instance.SetCurrentUser(user);
        }
    }
}