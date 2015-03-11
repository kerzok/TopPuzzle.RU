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

        [HttpGet]
        public ActionResult Login(ScoreModel model) {
            TempData["scoreModel"] = model;
            return View();
        }

        [HttpPost]
        public ActionResult Login(BaseModel model, string returnUrl) {
            if (!ModelState.IsValid) return View();
            if (model.Authenticate()) {
                if (TempData["scoreModel"] != null) {
                    ((ScoreModel)TempData["scoreModel"]).SaveScore();
                    TempData["scoreModel"] = null;
                }
                if (!String.IsNullOrEmpty(returnUrl)) {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Cabinet");
            }
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model) {
            if (!ModelState.IsValid) return View(model);
            model = model.RegisterNewUser();
            if (!model.HasError) {
                if (TempData["scoreModel"] == null) return RedirectToAction("Cabinet", "Account");
                ((ScoreModel)TempData["scoreModel"]).SaveScore();
                TempData["scoreModel"] = null;
                return RedirectToAction("Cabinet", "Account");
            }
            ModelState.AddModelError(model.Error.Key, model.Error.Value);
            return View(model);
        }

        [UserAuthorize]
        public ActionResult Logout() {
            ApplicationFacade.Instance.RemoveAuthCookie();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Cabinet(int? userId) {
            if (!userId.HasValue) userId = ApplicationFacade.Instance.GetCurrentUser().Id;
            ViewBag.User = ApplicationFacade.Instance.UserManager.GetUserById(userId.Value);
            return View();
        }

        public ActionResult MyPuzzle(CatalogModel model) {
            model.GetCatalogForCabinet();
            var view = RenderViewToString("MyPuzzle", model);
            return Json(new { view, model.CurrentPage, model.PageCount }, JsonRequestBehavior.AllowGet);
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
            return Json(new{data="ok", fileName = ChangeUserDataModel.ChangeAvatar(file)}, JsonRequestBehavior.AllowGet);
        }
    }
}