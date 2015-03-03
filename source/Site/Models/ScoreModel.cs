using System;
using System.Collections.Generic;
using System.Linq;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;

namespace Toppuzzle.Site.Models {
    public class ScoreModel : BaseModel {
        public List<Tuple<Score, string, int>> ScoresList { get; set; }

        public ScoreModel GetScores(int complexity) {
            var af = ApplicationFacade.Instance;
            var scoresList = af.ScoreManager.GetScores(complexity);
            ScoresList = scoresList.Select(score => new Tuple<Score, string, int>(score, 
                af.UserManager.GetUserNameById(score.UserId), 
                af.PictureManager.GetPictureByPictureId(score.PictureId).Id)).ToList();
            return this;
        }

        public void SaveScore(string time, string complexity, string puzzleId) {
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