using AA.PMTOGO.DAL;
using AA.PMTOGO.Infrastructure.Interfaces;
using AA.PMTOGO.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AA.PMTOGO.Services
{
    public class ServiceManagement : IServiceManagement
    {
        RequestDAO _requestDAO = new RequestDAO();
        UsersDAO _authNDAO = new UsersDAO();


        public async Task<Result> CreateRequest(Service service, string username, string comments, string frequency)
        {

            Result res = await _authNDAO.GetUser(username);
            User user = (User)res.Payload!;


            //get user info
            string firstName =  user!.FirstName;
            string lastName = user!.LastName;
            string propertyManagerName = firstName+ " " + lastName;
            string propertyManagerEmail = username;

            Guid serviceRequestId = Guid.NewGuid();
            //need property manager info
            ServiceRequest request = new ServiceRequest(serviceRequestId, service.ServiceName, service.ServiceType, service.ServiceDescription, frequency, comments, service.ServiceProvider, service.ServiceProviderEmail,
                propertyManagerEmail, propertyManagerName);

            Result result = await AddRequest(request);
            return result;
        }
        public async Task<Result> AddRequest(ServiceRequest request)
        {
            Result result = await _requestDAO.AddRequest(request.RequestId, request.ServiceName, request.ServiceType, request.ServiceDescription,
                request.ServiceFrequency, request.Comments, request.ServiceProviderEmail, request.ServiceProviderName, request.PropertyManagerEmail, request.PropertyManagerName);
            return result;  

        }
        public async Task<Result> GatherServices()
        {
            Result result = await _requestDAO.GetServices();
            return result;
        }
        public async Task<Result> GatherUserServices(string username)
        {
            Result result = await _requestDAO.GetUserService(username, username);
            return result;
        }

        public async Task<Result> CreateService(Service service)
        {
            Result result = await _requestDAO.AddService(service.ServiceName, service.ServiceType, service.ServiceDescription,
                service.ServiceProviderEmail, service.ServiceProvider);
            return result;
        }
        public async Task<Result> RateService(Guid id, int rate)
        {
            Result result = new Result();
            if (CheckRate(rate))
            {
                result = await _requestDAO.RateUserServices(id, rate);
                return result;
            }
            result.IsSuccessful = false;
            result.ErrorMessage = "Rate is out of range. Enter a number between 1-5.";
            return result;
        }
        public bool CheckRate(int rate)
        {
            if (rate > 5)
            {
                return false;
            }
            return true;
        }
    }
}
