using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toppuzzle.Model.DataMapping;
using Toppuzzle.Model.Managers;

namespace Toppuzzle.Test {
    public class Repositories {
        public PictureManager PictureManager { get; private set; }
        public ScoreManager ScoreManager { get; private set; }
        public UserManager UserManager { get; private set; }

        public Repositories(DapperSqlMapper mapper)
        {
            PictureManager = new PictureManager(mapper);
            UserManager = new UserManager(mapper);
            ScoreManager = new ScoreManager(mapper);
        }
    }
}
