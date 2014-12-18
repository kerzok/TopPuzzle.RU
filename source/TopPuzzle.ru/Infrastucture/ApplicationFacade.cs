using System.Configuration;
using System.Web;
using Model.DataMapping;
using Model.Managers;

namespace TopPuzzle.ru.Infrastucture {
    public class ApplicationFacade {
        public AuthManager AuthManager { get; set; }
        public ScoreManager ScoreManager { get; set; }

        private ApplicationFacade() {
            var mapper = new DapperSqlMapper(ConfigurationManager.ConnectionStrings["toppuzzle"].ConnectionString);
            AuthManager = new AuthManager(mapper);
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