using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Model.Entities;

namespace Model.Managers {
    public class SignInManager : SignInManager<Account, int> {
        public SignInManager(UserManager<Account, int> userManager, 
            IAuthenticationManager authenticationManager) : base(userManager, authenticationManager) {
        }

        public static SignInManager Create(IdentityFactoryOptions<SignInManager> options, IOwinContext context) {
            return new SignInManager(context.GetUserManager<UserManager>(), context.Authentication);
        }
    }
}
