using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Toppuzzle.Model.Entities;

namespace Toppuzzle.Site.Models {
    public class PictureViewModel : BaseViewModel {
        public string Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Source { get; set; }
        public int Complexity { get; set; }
        public IEnumerable<string> Parts { get; set; }
        public IEnumerable<int> RandomList { get; set; }

        public Score Score { get; set; }
    }
}