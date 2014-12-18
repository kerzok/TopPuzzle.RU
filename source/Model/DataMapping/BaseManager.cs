using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataMapping {
    public class BaseManager {
        public BaseManager(ISqlMapper sqlMapeer) {
            SqlMapper = sqlMapeer;
        }
        protected ISqlMapper SqlMapper { get; private set; }
    }
}
