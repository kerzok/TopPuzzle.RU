using System;
using System.Web.Mvc;
using System.Web.UI;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Attributes;
using Toppuzzle.Site.Infrastucture;
using Toppuzzle.Site.Models;

namespace Toppuzzle.Site.Controllers {
    public class AccountController : Controller {
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

        /*[Authorize]
        public ActionResult Cabinet() {
            var af = ApplicationFacade.Instance;
            var user = af.GetCurrentUser();
            var countStylystOrders = user.UserType == (int)UserType.Admin
                ? af.OrderManager.GetCountNotProcessedStylystOrders()
                : -1;
            return System.Web.UI.WebControls.View(new CabinetViewModel() {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Sex = user.Sex,
                Description = user.Description,
                UserType = user.UserType,
                CountStylystOrders = countStylystOrders
            });
        }

        [HttpPost]
        [Authorize]
        public JsonResult SetUserInfo(string name) {
            var af = ApplicationFacade.Instance;
            var user = af.GetCurrentUser();
            af.SetCurrentUser(af.UserManager.UpdateUserInfo(user.Id, firstName, lastName, description, sex));
            return Json(new { result = 1 });
        }*/

        private static bool Authenticate(string username, string password) {
            var af = ApplicationFacade.Instance;
            var user = af.UserManager.GetUserByLoginAndPassword(username, password);
            if (user == null) return false;
            af.SetCurrentUser(user);
            return true;
        }
    }
}