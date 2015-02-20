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
        private const int ItemPerPage = 9;

        public ViewResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(BaseViewModel model, string returnUrl) {
            if (!ModelState.IsValid) return View();
            if (Authenticate(model.Login, model.Password)) {
                return Redirect(!String.IsNullOrEmpty(returnUrl) ? returnUrl : new Page().ResolveUrl("~/"));
            }
            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
        }

        public ActionResult Register() {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var af = ApplicationFacade.Instance;
            var user = af.UserManager.GetUserByLogin(model.Login);
            if (user != null) {
                ModelState.AddModelError("UserLogin", "Пользователь с таким логином уже существует");
                return View(model);
            }
            if (!model.Password.Equals(model.ConfirmPassword)) {
                ModelState.AddModelError("RepeatPassword", "Пароли не совпадает");
                return View(model);
            }

            user = af.UserManager.InsertAccountToDatabase(new User {
                Email = model.Email,
                Password = model.Password,
                UserName = model.Login
            });
            af.SetCurrentUser(user);

            return RedirectToAction("Index", "Home");
        }

        [UserAuthorize]
        public ActionResult Logout() {
            ApplicationFacade.Instance.RemoveAuthCookie();
            return RedirectToAction("Index", "Home");
        }

        [UserAuthorize]
        public ActionResult Cabinet() {
            var af = ApplicationFacade.Instance;
            var user = af.GetCurrentUser();
            
            //var puzzle = af.PuzzleManager.GetPuzzlesByUserId(user.Id);
            return View(new CabinetViewModel {
                User = user,
                PuzzlesForUser = null
            });
        }

        [UserAuthorize]
        public ActionResult MyPuzzle(int page = 1) {
            var af = ApplicationFacade.Instance;
            var scores = af.ScoreManager.GetUserScoreById(af.GetCurrentUser().Id);
            var pictures = (from score in scores
                let picture = af.PictureManager.GetPictureByPictureId(score.PictureId)
                let path = Server.MapPath("~/Content/Puzzles/" + picture.Picture)
                let image = new Bitmap(path)
                select new PictureViewModel {
                    Complexity = score.Complexity,
                    Id = picture.PictureId,
                    Height = image.Height,
                    Source = "~/Content/Puzzles/" + picture.Picture,
                    Score = score
                });
            var picturesList = pictures.Skip((page - 1)*ItemPerPage).ToList();
            var totalPages = (pictures.Count() / ItemPerPage) + 1;
            var result = new CatalogViewModel {
                Pictures = picturesList,
                CurrentPage = page,
                PageCount = ApplicationFacade.Instance.PictureManager.GetTotalPages()
            };
            var view = RenderViewToString("MyPuzzle", result);
            return Json(new { view, CurrentPage = page, PageCount = totalPages }, JsonRequestBehavior.AllowGet);
        }

        [UserAuthorize]
        public ActionResult Settings() {
            return PartialView();
        }

        [UserAuthorize]
        public ActionResult ChangePassword() {
            return PartialView();
        }

        [UserAuthorize]
        public ActionResult ChangeEmail() {
            return PartialView();
        }

        [HttpPost]
        [UserAuthorize]
        public ActionResult ChangePassword(string OldPassword, string NewPassword, string ConfirmPassword) {
            var currentUser = ApplicationFacade.Instance.GetCurrentUser();
            if (!currentUser.Password.Equals(OldPassword)) return Json(new { data = "неверный старый пароль" }, JsonRequestBehavior.AllowGet);
            if (!NewPassword.Equals(ConfirmPassword)) return Json(new { data = "пароли не совпадают" }, JsonRequestBehavior.AllowGet);
            currentUser.Password = NewPassword;
            ApplicationFacade.Instance.UserManager.UpdateUser(currentUser);
            ApplicationFacade.Instance.SetCurrentUser(currentUser);
            return Json(new { data = "ok"}, JsonRequestBehavior.AllowGet);
        }

        [UserAuthorize]
        [HttpPost]
        public ActionResult ChangeEmail(string Email) {
            var user = ApplicationFacade.Instance.GetCurrentUser();
            user.Email = Email;
            ApplicationFacade.Instance.UserManager.UpdateUser(user);
            ApplicationFacade.Instance.SetCurrentUser(user);
            return Json(new{data="ok"}, JsonRequestBehavior.AllowGet);
        }

        [UserAuthorize]
        public ActionResult ChangeAvatar() {
            var currentUser = ApplicationFacade.Instance.GetCurrentUser();
            if (Request.Files.Count <= 0) return PartialView();
            var file = Request.Files[0];
            if (file == null || file.ContentLength <= 0) return PartialView();
            Directory.CreateDirectory(Server.MapPath("~/Content/Users/" + currentUser.Id + "/"));
            var picture = "avatar." + file.FileName.Substring(file.FileName.IndexOf('.') + 1);
            var fileName = "~/Content/Users/" + currentUser.Id + "/" + picture;
            fileName = Server.MapPath(fileName);
            file.SaveAs(fileName);
            currentUser.HasAvatar = true;
            currentUser.Avatar = picture;
            ApplicationFacade.Instance.UserManager.UpdateUser(currentUser);
            ApplicationFacade.Instance.SetCurrentUser(currentUser);
            return Json(new{data="ok", fileName}, JsonRequestBehavior.AllowGet);
        }

        private static bool Authenticate(string username, string password) {
            var af = ApplicationFacade.Instance;
            var user = af.UserManager.GetUserByLoginAndPassword(username, password);
            if (user == null) return false;
            af.SetCurrentUser(user);
            return true;
        }
    }
}