using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
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

        private readonly ILogger? _logger;

        public UsageAnalysisIntergrationTest( ILogger? logger)
        {
            _logger = logger;
        }
        [TestMethod]
        public async Task GetData_PASS()
        {
          /*  //arrange


            //act

            Result result = await _logger.GetAnalysisLogs("Authenticate");
            Console.WriteLine(result.Payload);
            bool actual = result.IsSuccessful;

            Assert.IsNotNull(result);
            Assert.IsTrue(actual);*/


        }
    }
}
