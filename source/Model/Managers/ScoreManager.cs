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

        public IEnumerable<Score> GetUserScoreById(int userId) {
            return SqlMapper.Execute<Score>("GetUserScoreById", new { userId });
        }

        public void InsertScore(Score score) {
            SqlMapper.Execute("InsertScore", score);
        }
    }
}