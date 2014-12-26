using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Entities;

namespace TopPuzzle.ru.Models
{
    public class ScoresViewModel
    {
        public List<Tuple<Score, string>> ScoresList { get; set; }
    }
}