using AA.PMTOGO.DAL.Interfaces;
using AA.PMTOGO.Logging;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Services;

public class ServiceFinder : IServiceFinder
{
    private readonly IServiceDAO _serviceDAO;
    private readonly ILogger _logger;
    public ServiceFinder(IServiceDAO serviceDAO, ILogger logger)
    {
        _serviceDAO = serviceDAO;
        _logger = logger;
    }

    public async Task<List<Service>> FindAllServices(string userQuery, int PageNumber, int PageLimit)
    {
        // log here

        List<Service> services = await _serviceDAO.FindServicesWithQuery(userQuery, PageNumber, PageLimit);

        if (services.Count > 0)
        {
            foreach (Service service in services)
            {
                if (service.ServiceName.Length > 50)
                {
                    service.ServiceName = service.ServiceName.Substring(0, 50);
                }
            }
        }

        return services;
    }
}
