using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Toppuzzle.Model.DataMapping {
    public class DapperSqlMapper : ISqlMapper {
        private readonly string _connectionString;

        public DapperSqlMapper(string connectionString) {
            _connectionString = connectionString;
        }

        public IEnumerable<T> Execute<T>(string procName, object parameters) {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                return connection.Query<T>(procName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> Query<T>(string query) {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                return connection.Query<T>(query);
            }
        }

        public void Query(string query)
        {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                connection.Query(query);
            }
        }

        public void Execute(string procName, object parameters) {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Dispose() {
            //none
        }
    }
}