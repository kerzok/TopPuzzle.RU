using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toppuzzle.Site.Models {
    public class ChangePasswordViewModel {
        [Display(Name="Старый пароль")]
        public string OldPassword { get; set; }
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
        [Display(Name = "Подтведите новый пароль")]
        public string ConfirmPassword { get; set; }
    }
}