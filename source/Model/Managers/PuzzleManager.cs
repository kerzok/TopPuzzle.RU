using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toppuzzle.Model.DataMapping;
using Toppuzzle.Model.Entities;

namespace Toppuzzle.Model.Managers {
    public class PuzzleManager : BaseManager {

        public PuzzleManager(ISqlMapper sqlMapeer)
            : base(sqlMapeer) {
        }

        public IEnumerable<Puzzle> GetPuzzlesByUserId(int id) {
            return SqlMapper.Execute<Puzzle>("GetPuzzlesByUserId", new {Id = id});
        }


    }
}
