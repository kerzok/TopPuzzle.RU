using System.Linq;
using Model.DataMapping;
using Model.Entities;

namespace Model.Managers {
    public class AuthManager : BaseManager{
        public AuthManager(ISqlMapper sqlMapeer) : base(sqlMapeer) {
        }

        public void InsertAccountToDatabase(Account account) {
            SqlMapper.Execute("InsertAccount", account);
        }

        public Account GetUserAccountById(int id) {
            return SqlMapper.Execute<Account>("GetUserAccountById", id).FirstOrDefault();
        }

        public Account GetUserAccountByLoginAndPassword(string login, string password) {
            return SqlMapper.Execute<Account>("GetUserAccountByLoginAndPassword", new {
                Login = login,
                Password = password
            }).SingleOrDefault();
        }

        public Account GetUserAccountByEmail(string email) {
            return SqlMapper.Execute<Account>("GetUserAccountByEmail", email).FirstOrDefault();
        }
    }
}
