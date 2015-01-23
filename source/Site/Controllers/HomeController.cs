using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;
using Toppuzzle.Site.Models;

namespace Toppuzzle.Site.Controllers {
    public class HomeController : Controller {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetScores(int complexity = 1) {
            var af = ApplicationFacade.Instance;
            var scoresList = af.ScoreManager.GetScores(complexity);
            var scoresListWithNames = scoresList.Select(score => new Tuple<Score, string>(score, af.UserManager.GetUserNameById(score.UserId))).ToList();
            return View(new ScoresViewModel { ScoresList = scoresListWithNames });
        }

        public ActionResult Catalog(int page = 1) {
            var pictures = (from picture in ApplicationFacade.Instance.PictureManager.GetAllPictures(page - 1)
                select "~/Content/Puzzles/" + picture.Picture
                into source
                let image = new Bitmap(Server.MapPath(source))
                select new PictureViewModel {Id = source.Substring(source.LastIndexOf('/') + 1, source.LastIndexOf('.') - (source.LastIndexOf('/') + 1)),
                    Height = image.Height, Width = image.Width, Source = source
                }).ToList();
            var result = new CatalogViewModel {
                Pictures = pictures,
                CurrentPage = page,
                PageCount = ApplicationFacade.Instance.PictureManager.GetTotalPages()
            };
            var view = RenderViewToString("Catalog", result);
            return Json(new { view, CurrentPage = page, PageCount = ApplicationFacade.Instance.PictureManager.GetTotalPages()});
        }

        [HttpPost]
        public void Random() {
            ApplicationFacade.Instance.PictureManager.GetRandomPicture();
        }

        //public ActionResult GetPuzzle(int id) {
            
        //}

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