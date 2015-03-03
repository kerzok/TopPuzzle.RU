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
        public ActionResult Index() {
            return View();
        }

        public ActionResult GetScores(int complexity = 1) {
            return View(new ScoreModel().GetScores(complexity));
        }

        public ActionResult Catalog(int page = 1) {
            var model = new CatalogModel().GetCatalogForMainPage(page);
            var view = RenderViewToString("Catalog", model);
            return Json(new { view, CurrentPage = page, model.PageCount });
        }

        public ActionResult Random() {
            return Json(new {
                pictureId = ApplicationFacade.Instance.PictureManager.GetRandomPicture().PictureId
            }, JsonRequestBehavior.AllowGet);
        }
    }
}