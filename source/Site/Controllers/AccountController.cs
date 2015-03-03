using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Attributes;
using Toppuzzle.Site.Infrastucture;
using Toppuzzle.Site.Models;

namespace Toppuzzle.Site.Controllers {
    public class AccountController : BaseController {
        

        public ViewResult Login() {
            return View();
        }
        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(BaseModel model, string returnUrl) {
            if (!ModelState.IsValid) return View();
            if (model.Authenticate()) {
                if (!String.IsNullOrEmpty(returnUrl)) {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Cabinet");
            }
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model) {
            if (!ModelState.IsValid) return View(model);
            model = model.RegisterNewUser();
            if (!model.HasError) return RedirectToAction("Index", "Home");
            ModelState.AddModelError(model.Error.Key, model.Error.Value);
            return View(model);
        }

        [UserAuthorize]
        public ActionResult Logout() {
            ApplicationFacade.Instance.RemoveAuthCookie();
            return RedirectToAction("Index", "Home");
        }

        [UserAuthorize]
        public ActionResult Cabinet() {
            ViewBag.User = ApplicationFacade.Instance.GetCurrentUser();
            return View();
        }

        [UserAuthorize]
        public ActionResult MyPuzzle(int page = 1) {
            var model = new CatalogModel().GetCatalogForCabinet(page);
            var view = RenderViewToString("MyPuzzle", model);
            return Json(new { view, CurrentPage = page, model.PageCount }, JsonRequestBehavior.AllowGet);
        }

        [UserAuthorize]
        public ActionResult Settings() {
            return PartialView();
        }

        [UserAuthorize]
        public ActionResult ChangePassword() {
            return PartialView(new ChangeUserDataModel());
        }

        [UserAuthorize]
        public ActionResult ChangeEmail() {
            return PartialView(new ChangeUserDataModel());
        }

        [HttpPost]
        [UserAuthorize]
        public ActionResult ChangePassword(ChangeUserDataModel model) {
            return PartialView(model.ChangePassword());
        }

        [HttpPost]
        [UserAuthorize]
        public ActionResult ChangeEmail(ChangeUserDataModel model) {
            return PartialView(model.ChangeEmail());
        }

        [UserAuthorize]
        public ActionResult ChangeAvatar() {
            if (Request.Files.Count <= 0) return PartialView();
            var file = Request.Files[0];
            if (file == null || file.ContentLength <= 0) return PartialView();
            return Json(new{data="ok", fileName = new ChangeUserDataModel().ChangeAvatar(file)}, JsonRequestBehavior.AllowGet);
        }
    }
}