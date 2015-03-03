using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toppuzzle.Site.Models {
    public class EditorViewModel : BaseModel {
        public string PictureId { get; set; }
        public int Complexity { get; set; }
    }
}