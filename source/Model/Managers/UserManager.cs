using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNet.Identity;
using Model.DataMapping;
using Model.Entities;

namespace Model.Managers {
    public class UserManager : UserManager<Account, int> {
        public UserManager(IUserStore<Account, int> store) : base(store) {
        }

        public static UserManager Create() {
            return new UserManager(new UserStore(ConfigurationManager.ConnectionStrings["toppuzzle"].ConnectionString));
        }

        /*public bool InsertAccountToDatabase(Account account) {
            try {
                //SqlMapper.Execute("InsertNewAccount", new{UserName = account.UserName, Password = account.Password, Email = account.Email});
                return true;
            } catch {
                return false;
            }
        }

        public Account GetUserAccountById(int id) {
            //return SqlMapper.Execute<Account>("GetUserAccountById", id).FirstOrDefault();
        }

        public Account GetUserAccountByLoginAndPassword(string login, string password) {
            /*return SqlMapper.Execute<Account>("GetUserAccountByLoginAndPassword", new {
                Login = login,
                Password = password
            }).SingleOrDefault();*/
        /*}

        public Account GetUserAccountByEmail(string email) {
            //return SqlMapper.Execute<Account>("GetUserAccountByEmail", email).FirstOrDefault();
        }*/
    }
}
