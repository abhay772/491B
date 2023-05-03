﻿using AA.PMTOGO.DAL;
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


        [TestMethod]
        public async Task GetData_PASS()
        {
            //arrange
            Result result = await logdao!.GetAnalysisLogs("CreateAccount");
            
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
    }
}
