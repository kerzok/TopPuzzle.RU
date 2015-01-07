using System.Collections.Generic;
using System.Linq;
using Toppuzzle.Model.DataMapping;
using Toppuzzle.Model.Entities;

namespace Toppuzzle.Model.Managers {
    public class ScoreManager : BaseManager {
        public ScoreManager(ISqlMapper sqlMapper) : base(sqlMapper) {
        }

        public IEnumerable<Score> GetScores(int complexity) {
            return SqlMapper.Execute<Score>("GetScores", new {complexity});
        }

        public IEnumerable<Score> GetUserScoreById(int id) {
            return SqlMapper.Execute<Score>("GetUserScoreById", new { id });
        }

        public void InsertNewScore(Score score) {
            SqlMapper.Execute("InsertNewScore", score);
        }
    }
}