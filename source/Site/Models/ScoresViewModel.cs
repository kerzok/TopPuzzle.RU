using System;
using System.Collections.Generic;
using Toppuzzle.Model.Entities;

namespace Toppuzzle.Site.Models {
    public class ScoresViewModel : BaseViewModel {
        public List<Tuple<Score, string, int>> ScoresList { get; set; }
    }
}