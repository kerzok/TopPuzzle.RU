using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities {
    public class Score {
        public int UserId { get; set; }
        public int PuzzleId { get; set; }
        public int Complexity { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }
    }
}
