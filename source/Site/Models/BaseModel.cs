using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Toppuzzle.Site.Infrastucture;

namespace Toppuzzle.Site.Models {
    public class BaseModel {
        [Required]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public bool Authenticate() {
            var af = ApplicationFacade.Instance;
            var user = af.UserManager.GetUserByLoginAndHash(Login, ApplicationFacade.GetPasswordHash(Password));
            if (user == null) return false;
            af.SetCurrentUser(user);
            return true;
        }
    }
}