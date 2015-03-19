using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Toppuzzle.Model.Entities;
using Toppuzzle.Site.Infrastucture;

namespace Toppuzzle.Site.Models {
    public class RegisterModel {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Значение {0} должно содержать не менее {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [System.ComponentModel.DataAnnotations.Compare("Password",
            ErrorMessage = "Пароль и его подтверждение не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Адрес электронной почты")]
        public string Email { get; set; }

        public KeyValuePair<string, string> Error;
        public bool HasError { get; private set; }

        public RegisterModel RegisterNewUser() {
            var af = ApplicationFacade.Instance;
            var user = af.UserManager.GetUserByLogin(Login);
            if (user != null) {
                Error = new KeyValuePair<string, string>("UserLogin", "Пользователь с таким логином уже существует");
                HasError = true;
                return this;
            }
            if (!Password.Equals(ConfirmPassword)) {
                Error = new KeyValuePair<string, string>("RepeatPassword", "Пароли не совпадает");
                HasError = true;
                return this;
            }

            user = af.UserManager.InsertUser(new User {
                Email = Email,
                PasswordHash = ApplicationFacade.GetPasswordHash(Password),
                UserName = Login
            });
            af.SetCurrentUser(user);
            return this;
        }
    }

    public class ChangeUserDataModel {
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
        [Display(Name = "Подтведите новый пароль")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
        public string Errors { get; set; }

        public bool Success { get; set; }

        public ChangeUserDataModel ChangePassword() {
            var currentUser = ApplicationFacade.Instance.GetCurrentUser();
            var newPasswordHash = ApplicationFacade.GetPasswordHash(OldPassword);
            if (!currentUser.PasswordHash.Equals(newPasswordHash)) {
                Errors = "неверный старый пароль";
                Success = false;
                return this;
            }
            if (!NewPassword.Equals(ConfirmPassword)) {
                Errors = "пароли не совпадают";
                Success = false;
                return this;
            }
            currentUser.PasswordHash = newPasswordHash;
            ApplicationFacade.Instance.UserManager.UpdateUser(currentUser);
            ApplicationFacade.Instance.SetCurrentUser(currentUser);
            Success = true;
            return this;
        }

        public ChangeUserDataModel ChangeEmail() {
            var user = ApplicationFacade.Instance.GetCurrentUser();
            user.Email = Email;
            ApplicationFacade.Instance.UserManager.UpdateUser(user);
            ApplicationFacade.Instance.SetCurrentUser(user);
            Success = true;
            return this;
        }

        public static string ChangeAvatar(HttpPostedFileBase file) {
            var currentUser = ApplicationFacade.Instance.GetCurrentUser();
            var folderPath = HostingEnvironment.ApplicationPhysicalPath + "Content/Users/" + currentUser.Id + "/";
            if (Directory.Exists(folderPath)) {
                foreach (var fileName in Directory.GetFiles(folderPath)) {
                    File.Delete(fileName);
                }
                Directory.Delete(folderPath);
            }
            Directory.CreateDirectory(folderPath);
            var picture = "avatar." + file.FileName.Substring(file.FileName.IndexOf('.') + 1);
            var path = folderPath + picture;
            file.SaveAs(path);
            currentUser.HasAvatar = true;
            currentUser.Avatar = picture;
            ApplicationFacade.Instance.UserManager.UpdateUser(currentUser);
            ApplicationFacade.Instance.SetCurrentUser(currentUser);
            return path;
        }
    }

    public class LoginViewModel : BaseModel {
        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}