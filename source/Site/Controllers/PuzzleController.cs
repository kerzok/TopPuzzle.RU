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

        public ActionResult Editor(string pictureId, int complexity) {
            return View(new EditorViewModel{Complexity = complexity, PictureId = pictureId});
        }

        public ActionResult GetPicture(string pictureId, int complexity) {
            return View(new PictureModel().GetPicture(pictureId, complexity));
        }

        [HttpPost]
        public void SaveScore(string time, string complexity, string puzzleId) {
            new ScoreModel().SaveScore(time, complexity, puzzleId);
        }
    }
}