using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;
using Toppuzzle.Site.Models;

namespace Toppuzzle.Site.Controllers {
    public class PuzzleController : Controller {
        // GET: Puzzle
        //public ActionResult Editor() {
        //    return View();
        //}

        public ActionResult Editor(string pictureId, int complexity) {
            return View(new EditorViewModel{Complexity = complexity, PictureId = pictureId});
        }

        public ActionResult Temp() {
            return RedirectToAction("GetPicture", new { pictureId = "15611247863", complexity = 1 });
        }

        public ActionResult GetPicture(string pictureId, int complexity) {
            var picture = ApplicationFacade.Instance.PictureManager.GetPictureByPictureId(pictureId);
            var source = "~/Content/Puzzles/" + picture.Picture;
            var image = new Bitmap(Server.MapPath(source));
            var cuttedImages = ApplicationFacade.Instance.PictureManager.CutImage(image, complexity);
            return View(new PictureViewModel
            {
                Complexity = complexity,
                Height = image.Height,
                Width = image.Width,
                Parts = cuttedImages,
                Source = source
            });
        }

        private string RenderViewToString(string viewName, object model) {
            ViewData.Model = model;
            using (var sw = new StringWriter()) {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}