using System.Collections.Generic;

namespace Toppuzzle.Model.DataMapping {
    public interface ISqlMapper {
        IEnumerable<T> Execute<T>(string procName, object parameters = null);
        void Execute(string procName, object parameters);
        IEnumerable<T> Query<T>(string query);
    }
}