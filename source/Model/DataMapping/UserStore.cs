using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Model.Entities;

namespace Model.DataMapping {
    public class UserStore : IUserStore<Account, int> {
        private readonly string _connectionString;

        public UserStore(string connectionString) {
            _connectionString = connectionString;
        }

        public void Dispose() {
        }

        public Task CreateAsync(Account user) {
            Action action =
                () => {
                    using (var connection = new SqlConnection(_connectionString)) {
                        connection.Open();
                        Dapper.SqlMapper.Execute(connection, "InsertNewAccount",
                            new {user.UserName, user.Password, user.Email}, commandType: CommandType.StoredProcedure);
                    }
                };
            return Task.Run(action);
        }

        public Task UpdateAsync(Account user) {
            Action action = () => {
                using (var connection = new SqlConnection(_connectionString)) {
                    connection.Open();
                    Dapper.SqlMapper.Execute(connection, "UpdateAccount",
                        new {user.Password, user.Email, user.Name}, commandType: CommandType.StoredProcedure);
                }
            };
            return Task.Run(action);
        }

        public Task DeleteAsync(Account user) {
            Action action = () => {
                using (var connection = new SqlConnection(_connectionString)) {
                    connection.Open();
                    Dapper.SqlMapper.Execute(connection, "DeleteAccount",
                        new {user.Id}, commandType: CommandType.StoredProcedure);
                }
            };
            return Task.Run(action);
        }

        public Task<Account> FindByIdAsync(int userId) {
            Func<Account> action = () => {
                using (var connection = new SqlConnection(_connectionString)) {
                    connection.Open();
                    return Dapper.SqlMapper.Query<Account>(connection, "FindAccountById",
                        new {Id = userId}, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            };
            return Task.Run(action);
        }

        public Task<Account> FindByNameAsync(string userName) {
            Func<Account> action = () => {
                using (var connection = new SqlConnection(_connectionString)) {
                    connection.Open();
                    return Dapper.SqlMapper.Query<Account>(connection, "FindAccountByUserName",
                        new {UserName = userName}, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
            };
            return Task.Run(action);
        }
    }
}
