using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;
using Toppuzzle.Site.Models;

namespace Toppuzzle.Site.Controllers {
    public class HomeController : BaseController {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetScores(int complexity = 1) {
            var af = ApplicationFacade.Instance;
            var scoresList = af.ScoreManager.GetScores(complexity);
            var scoresListWithNames = scoresList.Select(score => new Tuple<Score, string, int>(score, af.UserManager.GetUserNameById(score.UserId), af.PictureManager.GetPictureByPictureId(score.PictureId).Id)).ToList();
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

        public ActionResult Random() {
            return Json(new{pictureId = ApplicationFacade.Instance.PictureManager.GetRandomPicture().PictureId}, JsonRequestBehavior.AllowGet);
        }        
    }
}