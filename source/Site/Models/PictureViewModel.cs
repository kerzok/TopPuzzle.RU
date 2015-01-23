using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Toppuzzle.Site.Models {
    public class PictureViewModel : BaseViewModel {
        public string Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Source { get; set; }
        public int Complexity { get; set; }
        public IEnumerable<Bitmap> Parts { get; set; } 
    }
}