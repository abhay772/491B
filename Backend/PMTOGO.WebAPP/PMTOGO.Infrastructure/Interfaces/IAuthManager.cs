﻿using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IAuthManager
    {
        Task<Result> Login(string username, string password);


    }
}
