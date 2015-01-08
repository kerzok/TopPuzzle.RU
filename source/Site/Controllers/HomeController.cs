using System.Drawing;
using System.Web.Mvc;
using Toppuzzle.Site.Helpers;

namespace Toppuzzle.Site.Controllers {
    public class HomeController : Controller {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About() {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact() {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}