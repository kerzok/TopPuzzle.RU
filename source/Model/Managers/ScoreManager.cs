using System.Collections.Generic;
using Model.DataMapping;
using Model.Entities;

namespace Model.Managers {
    public class ScoreManager : BaseManager {
        public ScoreManager(ISqlMapper sqlMapeer) : base(sqlMapeer) {
        }

        public IEnumerable<Score> GetTopScore(int complexity) {
            return SqlMapper.Execute<Score>("GetTopScore", complexity);
        }
 
        
    }
}
