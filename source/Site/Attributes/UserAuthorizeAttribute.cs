using System.Web;
using System.Web.Mvc;
using Toppuzzle.Site.Infrastucture;

namespace Toppuzzle.Site.Attributes {
    public class UserAuthorizeAttribute : AuthorizeAttribute {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) {
            //filterContext.Result = new HttpUnauthorizedResult(); // Try this but i'm not sure
            filterContext.Result = new RedirectResult("~/login");
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext) {
            return ApplicationFacade.Instance.IsAuthenticated();
        }
    }
}