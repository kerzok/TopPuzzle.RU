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
        public ActionResult Editor(PictureModel model) {
            return View(model.GetPicture());
        }

        [HttpPost]
        public ActionResult SaveScore(ScoreModel model, int whereTo) {
            model.SaveScore();
            return whereTo == 1 ? RedirectToAction("Index", "Home") : RedirectToAction("Cabinet", "Account");
        }
    }
}