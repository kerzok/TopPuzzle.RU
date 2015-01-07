using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Controllers;

namespace Toppuzzle.Site.Models {
    public class CabinetViewModel : BaseViewModel {
        public User User { get; set; }
        public IEnumerable<Puzzle> PuzzlesForUser { get; set; }
    }
}