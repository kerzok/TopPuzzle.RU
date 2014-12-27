using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;
using Toppuzzle.Site.Models;

namespace Toppuzzle.Site.Controllers {
    public class PuzzleController : Controller {
        // GET: Puzzle
        public ActionResult Editor() {
            return View();
        }

        public ActionResult GetScores(int complexity = 1) {
            var scoresListWithNames = new List<Tuple<Score, string>>();
            var af = ApplicationFacade.Instance;
            var scoresList = af.ScoreManager.GetScores(complexity);
            foreach (var score in scoresList) {
                scoresListWithNames.Add(new Tuple<Score, string>(score, af.ScoreManager.GetUserNameById(score.UserId)));
            }
            return View(new ScoresViewModel {ScoresList = scoresListWithNames});
        }
    }
}