using System;
using System.Collections.Generic;
using System.Linq;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;

namespace Toppuzzle.Site.Models {
    public class ScoreModel : BaseModel {
        public User User { get; set; }
        public int Time { get; set; }
        public int PictureId { get; set; }
        public DateTime Date { get; set; }
        public int Complexity { get; set; }
        public bool HasSaved { get; set; }

        public static List<ScoreModel> GetScores(int complexity) {
            var af = ApplicationFacade.Instance;
            var scoresList = af.ScoreManager.GetScores(complexity);
            var result = scoresList.Select(score => new ScoreModel {
                Date = score.Date,
                User = af.UserManager.GetUserById(score.UserId),
                PictureId = score.PictureId,
                Time = score.Time
            }).ToList();
            return result;
        }

        public ScoreModel SaveScore() {
            if (HasSaved) return this;
            var user = ApplicationFacade.Instance.GetCurrentUser();
            if (user == null) return this;
            var score = new Score {
                Complexity = Complexity,
                Date = DateTime.Today,
                PictureId = PictureId,
                Time = Time,
                UserId = user.Id
            };
            user.Rating += score.Complexity*20;
            ApplicationFacade.Instance.ScoreManager.InsertScore(score);
            ApplicationFacade.Instance.UserManager.UpdateUser(user);
            ApplicationFacade.Instance.SetCurrentUser(user);
            HasSaved = true;
            return this;
        }
    }
}