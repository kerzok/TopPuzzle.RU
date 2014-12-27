using System;

namespace Toppuzzle.Model.Entities {
    public class Score {
        public int UserId { get; set; }
        public int PuzzleId { get; set; }
        public int Complexity { get; set; }
        public int Time { get; set; }
        public DateTime Date { get; set; }
    }
}