using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IUserManagement
    {
        Task<Result> CreateAccount(string email, string password, string firstname, string lastname, string role); 
        Task<Result> DeleteAccount(string username);
        string GenerateSalt();
        string EncryptPassword(string password, string salt);
    }
}
