using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Toppuzzle.Site.Models {
    public class ChangeEmailViewModel {
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }
    }
}