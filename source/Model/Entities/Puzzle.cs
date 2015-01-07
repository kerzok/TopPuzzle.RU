using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toppuzzle.Model.Entities {
    public class Puzzle {
        string Picture { get; set; }
        int Complexity { get; set; }
        int[] Partition { get; set; }
    }
}
