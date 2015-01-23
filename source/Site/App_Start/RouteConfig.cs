using System.Web.Mvc;
using System.Web.Routing;

namespace Toppuzzle.Site {
    public static class RouteHelper {
        public static Route Map(this RouteCollection routes, string url, string controller, string action) {
            return routes.MapRoute(null, url, new {controller, action}
                );
        }

        public static Route Map(this RouteCollection routes, string url, string controller, string action, string method) {
            return routes.MapRoute(null, url, new {
                controller,
                action
            }, new {
                method = new HttpMethodConstraint(method)
            }
                );
        }
    }

    public static class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Map("puzzle", "Puzzle", "Editor");
            routes.Map("puzzle/get", "Puzzle", "GetPictures");
            routes.Map("register", "Account", "Register");
            routes.Map("login", "Account", "Login");
            routes.Map("cabinet", "Account", "Cabinet");
            routes.Map("cabinet/default", "Account", "MyPuzzle");
            routes.Map("cabinet/settings", "Account", "Settings");

            routes.Map("cabinet/settings/default", "Account", "ChangeEmail");
            routes.Map("cabinet/settings/password", "Account", "ChangePassword");
            
            routes.Map("cabinet/settings/avatar", "Account", "ChangeAvatar");

            routes.Map("scores", "Home", "GetScores");
            routes.Map("home/getdata", "Home", "Catalog");
            routes.Map("home/random", "Home", "Random");

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}