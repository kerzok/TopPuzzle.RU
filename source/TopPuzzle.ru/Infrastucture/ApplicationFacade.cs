using System.Configuration;
using System.Web;
using Microsoft.AspNet.Identity;
using Model.DataMapping;
using Model.Entities;
using Model.Managers;

namespace TopPuzzle.ru.Infrastucture {
    public class ApplicationFacade {
        public UserManager UserManager { get; set; }
        public ScoreManager ScoreManager { get; set; }
        public SignInManager SignInManager { get; set; }
        

        private ApplicationFacade() {
            var mapper = new DapperSqlMapper(ConfigurationManager.ConnectionStrings["toppuzzle"].ConnectionString);
            ScoreManager = new ScoreManager(mapper);
        }

        public static ApplicationFacade Instance {
            get {
                var af = HttpContext.Current.Items["ApplicationFacade"] as ApplicationFacade;
                if (af != null) return af;
                af = new ApplicationFacade();
                HttpContext.Current.Items["ApplicationFacade"] = af;
                return af;
            }
        }
    }
}