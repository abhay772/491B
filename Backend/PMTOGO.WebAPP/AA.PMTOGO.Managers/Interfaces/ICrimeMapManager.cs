using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Managers.Interfaces
{
    public interface ICrimeMapManager
    {
        Task<Result> AddCrimeAlert(string email, string name, string description, string time, string date, string x, string y);
        Task<Result> DeleteCrimeAlert(string email, string id);
        Task<Result> EditCrimeAlert(string email, string id, string name, string description, string time, string date, string x, string y);
        Task<List<CrimeAlert>> GetCrimeAlerts();
        Task<CrimeAlert> ViewCrimeAlert(string email, string id);
    }
}
