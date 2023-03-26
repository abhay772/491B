using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Infrastructure.Interfaces
{
    public interface IAccountManager
    {
        Task<Result> RegisterUser(string email, string password, string firstname, string lastname, string role);

        Task<Result> DeleteUserAccount(string email);
    }
}
