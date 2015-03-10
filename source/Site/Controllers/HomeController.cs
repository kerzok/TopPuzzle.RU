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
            return View(ScoreModel.GetScores(complexity));
        }

        public ActionResult Catalog(CatalogModel model) {
            var view = RenderViewToString("Catalog", model.GetCatalogForMainPage());
            return Json(new { view, model.CurrentPage, model.PageCount });
        }

        public ActionResult Random() {
            return Json(new {
                Id = ApplicationFacade.Instance.PictureManager.GetRandomPicture().Id
            }, JsonRequestBehavior.AllowGet);
        }
    }
}