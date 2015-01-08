using System;
using System.Configuration;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using Toppuzzle.Model.DataMapping;
using Toppuzzle.Model.Entities;
using Toppuzzle.Model.Managers;

namespace Toppuzzle.Site.Infrastucture {
    public class ApplicationFacade {
        private const string UserCoockieKey = "userCoockie";

        private ApplicationFacade() {
            var mapper = new DapperSqlMapper(ConfigurationManager.ConnectionStrings["toppuzzle"].ConnectionString);
            ScoreManager = new ScoreManager(mapper);
            UserManager = new UserManager(mapper);
            PuzzleManager = new PuzzleManager(mapper);
            PictureManager = new PictureManager(mapper);
        }

        public UserManager UserManager { get; set; }
        public ScoreManager ScoreManager { get; set; }
        public PuzzleManager PuzzleManager { get; set; }
        public PictureManager PictureManager { get; set; }

        public static ApplicationFacade Instance {
            get {
                var af = HttpContext.Current.Items["ApplicationFacade"] as ApplicationFacade;
                if (af != null) return af;
                af = new ApplicationFacade();
                HttpContext.Current.Items["ApplicationFacade"] = af;
                return af;
            }
        }

        public User GetCurrentUser() {
            var json = GetAuthCookie(UserCoockieKey);
            if (json == null) return null;
            var user = JsonConvert.DeserializeObject<User>(json);
            return user;
        }

        public bool IsAuthenticated() {
            var user = GetCurrentUser();
            return user != null;
        }

        public void SetCurrentUser(User user) {
            SetAuthCookie(UserCoockieKey, user.Id.ToString(), JsonConvert.SerializeObject(user));
        }

        public void SetCurrentUser(User user, DateTime expirationDate) {
            SetAuthCookie(UserCoockieKey, user.Id.ToString(), JsonConvert.SerializeObject(user), expirationDate);
        }

        private static void SetAuthCookie(string key, string name, string data) {
            SetAuthCookie(key, name, data, DateTime.Now.AddDays(30));
        }

        private static void SetAuthCookie(string key, string name, string data, DateTime expirationDate) {
            var ticket = new FormsAuthenticationTicket(
                1,
                name,
                DateTime.Now,
                expirationDate,
                true,
                data,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(key, encryptedTicket);

            HttpContext.Current.Request.Cookies.Remove(key);
            HttpContext.Current.Response.Cookies.Remove(key);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        private static string GetAuthCookie(string key) {
            var cookie = HttpContext.Current.Request.Cookies[key];

            if (cookie == null) return null;
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            if (ticket == null) return null;
            if (ticket.Expired)
                return null;

            var ret = ticket.UserData;
            return ret;
        }

        public void RemoveAuthCookie() {
            var currentUserCookie = HttpContext.Current.Request.Cookies[UserCoockieKey];
            HttpContext.Current.Response.Cookies.Remove(UserCoockieKey);
            if (currentUserCookie == null) return;
            currentUserCookie.Expires = DateTime.Now.AddDays(-10);
            currentUserCookie.Value = null;
            HttpContext.Current.Response.SetCookie(currentUserCookie);
        }
    }
}