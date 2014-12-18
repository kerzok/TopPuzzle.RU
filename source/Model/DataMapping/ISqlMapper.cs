using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataMapping {
    public interface ISqlMapper {
        IEnumerable<T> Execute<T>(string procName, object parameters = null);
        void Execute(string procName, object parameters);
        IEnumerable<T> Query<T>(string query);
    }
}
