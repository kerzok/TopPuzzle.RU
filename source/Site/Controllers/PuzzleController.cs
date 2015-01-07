using System;
using System.Collections.Generic;
using System.Linq;
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
            var af = ApplicationFacade.Instance;
            var scoresList = af.ScoreManager.GetScores(complexity);
            var scoresListWithNames = scoresList.Select(score => new Tuple<Score, string>(score, af.UserManager.GetUserNameById(score.UserId))).ToList();
            return View(new ScoresViewModel {ScoresList = scoresListWithNames});
        }
    }
}