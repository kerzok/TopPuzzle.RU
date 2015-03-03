using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;

namespace Toppuzzle.Site.Models {
    public class PictureModel : BaseModel {
        public string Id { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Source { get; set; }
        public int Complexity { get; set; }
        public IEnumerable<string> Parts { get; set; }
        public IEnumerable<int> RandomList { get; set; }
        public Score Score { get; set; }

        public PictureModel GetPicture(string pictureId, int complexity) {
            Complexity = complexity;
            var picture = ApplicationFacade.Instance.PictureManager.GetPictureByPictureId(pictureId);
            Source = @"~/Content/Puzzles/" + picture.Picture;
            var image = new Bitmap(HostingEnvironment.ApplicationPhysicalPath + @"Content/Puzzles/" + picture.Picture);
            Height = image.Height;
            Width = image.Width;
            Parts = ApplicationFacade.Instance.PictureManager.CutImage(image, complexity);
            RandomList = ApplicationFacade.Instance.PictureManager.GetRandomList(complexity);
            return this;
        }
    }
}