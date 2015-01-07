using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toppuzzle.Model.Entities;

namespace Toppuzzle.Site.Models {
    public class MyPuzzleViewModel {
        public IEnumerable<Puzzle> PuzzlesForUser { get; set; } 
    }
}