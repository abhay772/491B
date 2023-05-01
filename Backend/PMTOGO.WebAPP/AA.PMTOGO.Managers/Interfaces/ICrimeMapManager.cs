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
        Task<Result> AddCrimeAlert(string email, string name, string location, string description, string time, string date, float x, float y);
        Task<Result> DeleteCrimeAlert(string email, int id);
        Task<Result> EditCrimeAlert(string email, int id, string name, string location, string description, string time, string date, float x, float y);
        Task<List<CrimeAlert>> GetCrimeAlerts();
        Task<CrimeAlert> ViewCrimeAlert(int id);
    }
}
