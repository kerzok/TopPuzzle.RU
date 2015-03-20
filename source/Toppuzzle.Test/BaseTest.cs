using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Toppuzzle.Model.DataMapping;

namespace Toppuzzle.Test {

    [TestClass]
    public abstract class BaseTest {
        protected TestHelper TestHelper { get; private set; }
        protected Repositories Repositories { get; private set; }
        private DapperSqlMapper Mapper { get; set; }

        [TestInitialize()]
        public void TestInitialize()
        {
            var connetctionString = ConfigurationManager.ConnectionStrings["toppuzzle"].ConnectionString;
            Mapper = new DapperSqlMapper(connetctionString);
            Repositories = new Repositories(Mapper);
            TestHelper = new TestHelper();
        }

        [TestCleanup()]
        public void TestCleanup() {
            var dllPath = AppDomain.CurrentDomain.BaseDirectory;
            var binPath = Directory.GetParent(dllPath).ToString();
            var path = Directory.GetParent(binPath).ToString();
            var clearScript = File.ReadAllText(path + @"\clear.sql");
            Mapper.Query(clearScript);
        }
    }
}
