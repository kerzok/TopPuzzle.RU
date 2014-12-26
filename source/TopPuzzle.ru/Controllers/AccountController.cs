using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Model.Entities;
using Model.Managers;
using TopPuzzle.ru.Infrastucture;
using TopPuzzle.ru.Models;

namespace TopPuzzle.ru.Controllers
{
    [Authorize]
    public class AccountController : Controller {
        private SignInManager _signInManager;
        private UserManager _userManager;

        public AccountController() {
        }

        public AccountController(UserManager userManager, SignInManager signInManager) {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public SignInManager SignInManager {
            get {
                return _signInManager ?? HttpContext.GetOwinContext().Get<SignInManager>();
            }
            private set {
                _signInManager = value;
            }
        }

        public UserManager UserManager {
            get {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set {
                _userManager = value;
            }
        }
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ConfirmRegister(RegisterViewModel model) {
            if (!ModelState.IsValid) return RedirectToAction("Register");
            var user = new Account {UserName = model.Login, Email = model.Email, Password = model.Password};
            var result = await UserManager.CreateAsync(user);
            if (!result.Succeeded) return RedirectToAction("Register");
            await SignInManager.SignInAsync(user, false, false);
            RedirectToAction("Index", "Home");
            return RedirectToAction("Register");
        }

        [AllowAnonymous]
        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            SignInManager.AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult ConfirmLogin(LoginViewModel model, string returnUrl) {
            if (!ModelState.IsValid) {
                return RedirectToAction("Login", "Account", model);
            }

            // Сбои при входе не приводят к блокированию учетной записи
            // Чтобы ошибки при вводе пароля инициировали блокирование учетной записи, замените на shouldLockout: true
            var result = SignInManager.PasswordSignIn(model.Login, model.Password, model.RememberMe, false);
            switch (result) {
                case SignInStatus.Success:
                    return Redirect(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                default:
                    ModelState.AddModelError("", "Неудачная попытка входа.");
                    return RedirectToAction("Login", "Account", model);
            }
            
        }
    }
}