using System.Data;
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
                SqlMapper.Execute<User>("InsertUser", new {user.UserName, user.Password, user.Email})
                    .FirstOrDefault();
        }

        public User GetUserById(int id) {
            return SqlMapper.Execute<User>("GetUserById", new {Id = id}).FirstOrDefault();
        }

        public User GetUserByLoginAndPassword(string login, string password) {
            return SqlMapper.Execute<User>("GetUserByLoginAndPassword", new {
                Login = login,
                Password = password
            }).FirstOrDefault();
        }

        public User GetUserByLogin(string login) {
            return SqlMapper.Execute<User>("GetUserByLogin", new {Login = login}).FirstOrDefault();
        }

        public User GetUserAccountByEmail(string email) {
            return SqlMapper.Execute<User>("GetUserByEmail", email).FirstOrDefault();
        }

        public void UpdateUser(User user) {
            SqlMapper.Execute("UpdateUser", new{user.Id, user.Email, user.Password, user.Avatar, user.HasAvatar, user.Rating});
        }
    }
}