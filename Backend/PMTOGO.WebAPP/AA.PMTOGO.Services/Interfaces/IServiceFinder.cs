using AA.PMTOGO.Models.Entities;

namespace AA.PMTOGO.Services.Interfaces;

public interface IServiceFinder
{
    Task<List<Service>> FindAllServices(string userQuery, int PageNumber, int PageLimit);
}