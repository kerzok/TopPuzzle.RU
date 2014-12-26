using System.Collections.Generic;
using System.Linq;
using Model.DataMapping;
using Model.Entities;

namespace Model.Managers {
    public class ScoreManager : BaseManager {
        public ScoreManager(ISqlMapper sqlMapper) : base(sqlMapper) {
        }

        public IEnumerable<Score> GetScores(int complexity) {
            return SqlMapper.Execute<Score>("GetScores", new {complexity});
        }

        public string GetUserNameById(int id)
        {
            return SqlMapper.Execute<string>("GetUserNameById", new {id}).FirstOrDefault();
        }

        public void InsertNewScore(Score score) {
            SqlMapper.Execute("InsertNewScore", score);
        }
    }
}
