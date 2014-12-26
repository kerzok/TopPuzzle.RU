using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataMapping {
    public class DapperSqlMapper : ISqlMapper {
        private readonly string _connectionString;

        public DapperSqlMapper(string connectionString ) {
            _connectionString = connectionString;
        }
        public IEnumerable<T> Execute<T>(string procName, object parameters) {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                return Dapper.SqlMapper.Query<T>(connection, procName, parameters, commandType: CommandType.StoredProcedure);
            }

        }

        public IEnumerable<T> Query<T>(string query) {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                return Dapper.SqlMapper.Query<T>(connection, query);
            }
        }
        public void Execute(string procName, object parameters) {
            using (var connection = new SqlConnection(_connectionString)) {
                connection.Open();
                Dapper.SqlMapper.Execute(connection, procName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public void Dispose() {
            //none
        }
    }
}
