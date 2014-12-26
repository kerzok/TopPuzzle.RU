using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TopPuzzle.ru {

    public static class RouteHelper {
        public static Route Map(this RouteCollection routes, string url, string controller, string action) {
            return routes.MapRoute(
               name: null,
               url: url,
               defaults: new { controller = controller, action = action }
           );
        }

        public static Route Map(this RouteCollection routes, string url, string controller, string action, string method) {
            return routes.MapRoute(
               name: null,
               url: url,
               defaults: new {
                   controller = controller, action = action
               },
               constraints: new {
                   method = new HttpMethodConstraint(method)
               }
           );
        }
    }
    public static class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Map("puzzle", "Puzzle", "Editor");
            routes.Map("register", "Account", "Register");

            routes.Map("scores", "Puzzle", "GetScores");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
