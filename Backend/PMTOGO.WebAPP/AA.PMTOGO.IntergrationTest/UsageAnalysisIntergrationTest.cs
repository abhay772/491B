using AA.PMTOGO.DAL;
using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using Microsoft.Extensions.Configuration;
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
        LoggerDAO logdao = new LoggerDAO();
        //private readonly ILogger? _logger;


        [TestMethod]
        public async Task GetData_PASS()
        {
            //arrange
            Result result = await logdao!.GetAnalysisLogs("Authentication");
            
            //act
            IDictionary<DateTime, int> data = (Dictionary<DateTime, int>)result.Payload!;
            foreach (var day in data)
            {
                Console.WriteLine($"{day.Key}: {day.Value}");
            }
            Console.WriteLine(data);
            bool actual = result.IsSuccessful;

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(actual);

        }
        //testing logging
        [TestMethod]
        public async Task GetLog_PASS()
        {
            //arrange
            Result result = new Result();
            result.IsSuccessful = false;
            result.ErrorMessage = "Testing Logger";
            Logger _logger = new Logger(logdao);
            await _logger!.Log("GetUserInfo", 4, LogCategory.Business, result);
            

            //act
            bool actual = result.IsSuccessful;

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(!actual);


        }
    }
}
