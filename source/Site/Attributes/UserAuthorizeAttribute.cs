using System.Web;
using System.Web.Mvc;
using Toppuzzle.Site.Infrastucture;

namespace Toppuzzle.Site.Attributes {
    public class UserAuthorizeAttribute : AuthorizeAttribute {
        protected override bool AuthorizeCore(HttpContextBase httpContext) {
            return ApplicationFacade.Instance.IsAuthenticated();
        }
    }
}