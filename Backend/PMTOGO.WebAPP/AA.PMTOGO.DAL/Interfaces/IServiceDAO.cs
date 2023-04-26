using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.DAL.Interfaces
{
    public interface IServiceDAO
    {
        Task<Result> GetServices();
        Task<Result> FindService(Guid id);
        Task<Result> AddService(Service service);
        Task<Result> DeleteService(Guid id);

    }
}
