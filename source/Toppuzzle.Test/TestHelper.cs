using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toppuzzle.Test {
    public class TestHelper {
        public Random Random;

        public TestHelper() {
            Random = new Random();
        }

        public string GenerateGuidString() {
            return Guid.NewGuid().ToString();
        }

        public string GenerateGuidString(int a) {
            return GenerateGuidString().Substring(0, a);
        }
    }
}
