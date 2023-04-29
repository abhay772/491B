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

        private readonly IConfiguration? _configuration;
        
        private readonly ILogger? _logger;
        private readonly string _connectionString;
        
        private readonly ILoggerDAO _loggerDAO;

        public UsageAnalysisIntergrationTest( ILogger? logger, ILoggerDAO loggerDAO, IConfiguration _configuration)
        {
            _logger = logger;
            _loggerDAO = loggerDAO;
            _connectionString = _configuration!.GetConnectionString("ServiceDbConnectionString")!;
        }
        

        [TestMethod]
        public async Task GetData_PASS()
        {
            //arrange
            LoggerDAO _loggerDAO = new LoggerDAO(_configuration!);

            //act

            Result result = await _loggerDAO!.GetAnalysisLogs("Authenticate");
            IDictionary<DateTime, int> data = (Dictionary<DateTime, int>)result.Payload!;
            Console.WriteLine(data);
            bool actual = result.IsSuccessful;

            Assert.IsNotNull(result);
            Assert.IsTrue(actual);


        }
    }
}
