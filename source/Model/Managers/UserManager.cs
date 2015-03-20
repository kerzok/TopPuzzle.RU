using System.Linq;
using Toppuzzle.Model.DataMapping;
using Toppuzzle.Model.Entities;

namespace Toppuzzle.Model.Managers {
    public class UserManager : BaseManager {
        public UserManager(ISqlMapper sqlMapeer)
            : base(sqlMapeer) {
        }

        public User InsertUser(User user) {
            return
                SqlMapper.Execute<User>("InsertUser", new {user.UserName, user.PasswordHash, user.Email})
                    .FirstOrDefault();
        }

        public User GetUserById(int id) {
            return SqlMapper.Execute<User>("GetUserById", new {Id = id}).FirstOrDefault();
        }

        public User GetUserByLoginAndHash(string login, int password) {
            return SqlMapper.Execute<User>("GetUserByLoginAndHash", new {
                Login = login,
                PasswordHash = password
            }).FirstOrDefault();
        }

        public User GetUserByLogin(string login) {
            return SqlMapper.Execute<User>("GetUserByLogin", new {Login = login}).FirstOrDefault();
        }

        public User GetUserAccountByEmail(string email) {
            return SqlMapper.Execute<User>("GetUserByEmail", email).FirstOrDefault();
        }

        public void UpdateUserAvatar(User user) {
            SqlMapper.Execute("UpdateUserAvatar", new{user.Id, user.Avatar});
        }

        public void UpdateUserEmail(User user) {
            SqlMapper.Execute("UpdateUserEmail", new { user.Id, user.Email});
        }

        public void UpdateUserPassword(User user) {
            SqlMapper.Execute("UpdateUserPassword", new { user.Id, user.PasswordHash});
        }

        public void UpdateUserRating(User user) {
            SqlMapper.Execute("UpdateUserRating", new { user.Id, user.Rating });
        }
    }
}