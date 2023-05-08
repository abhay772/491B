using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Services
{
    //input validation, error handling , logging
    public class ServiceManagement: IServiceManagement
    {
        ServiceDAO _serviceDAO = new ServiceDAO();
        public async Task<Result> GatherServices()
        {
            Result result = await _serviceDAO.GetServices();
            return result;
        }

        public async Task<Result> CreateService(Service service)
        {
             Result result = await _serviceDAO.AddService(service.ServiceName, service.ServiceType, service.ServiceDescription,
                 service.ServiceProviderEmail, service.ServiceProvider);
             return result;
        }

        public async Task<Result> RemoveService(Service service)
        {
            Result result = await _serviceDAO.DeleteService(service.ServiceName, service.ServiceType, service.ServiceProvider);
            return result;
        }
    }
}
