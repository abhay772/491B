﻿using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Services.Interfaces
{
    public interface ICrimeMapService
    {
        Task<Result> AddAlert(CrimeAlert alert);
        Task<Result> CheckAlert(string email);
        Task<Result> DeleteAlert(string email, int id);
        Task<Result> EditAlert(string email, int id, CrimeAlert alert);
        Task<List<CrimeAlert>> GetAlerts();
    }
}
