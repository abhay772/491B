using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.IntergrationTest
{
    [TestClass]
    public class UsageAnalysisIntergrationTest
    {
        [TestMethod]
        public async Task GetData_PASS()
        {
            //arrange
            var dao = new LoggerDAO();

            //act

            Result result = await dao.GetAnalysisLogs("Authenticate");
            Console.WriteLine(result.Payload);
            bool actual = result.IsSuccessful;

            Assert.IsNotNull(result);
            Assert.IsTrue(actual);


        }
    }
}
