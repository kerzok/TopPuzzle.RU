using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Toppuzzle.Site.Models {
    public class CatalogViewModel : BaseViewModel {
        public IEnumerable<PictureViewModel> Pictures { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}