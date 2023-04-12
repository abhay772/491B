using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;

namespace AA.PMTOGO.Services
{
    //input validation, error handling , logging
    public class UserServiceManagement : IUserServiceManagement
    {
        UserServiceDAO _userServiceDAO = new UserServiceDAO();
        UsersDAO _authNDAO = new UsersDAO();
        public async Task<Result> CreateRequest(ServiceRequest service, string username)
        {

            Result res = await _authNDAO.GetUser(username);
            User user = (User)res.Payload!;


            //get user info
            string firstName =  user!.FirstName;
            string lastName = user!.LastName;
            string propertyManagerName = firstName+ " " + lastName;

            Guid serviceRequestId = Guid.NewGuid();
            //need property manager info
            ServiceRequest request = new ServiceRequest(serviceRequestId, service.ServiceName, service.ServiceType, service.ServiceDescription, service.ServiceFrequency, service.Comments, service.ServiceProviderName, service.ServiceProviderEmail,
                username, propertyManagerName);

            Result result = await AddRequest(request);
            return result;
        }
        public async Task<Result> AddRequest(ServiceRequest request)
        {
            Result result = await _userServiceDAO.AddServiceRequest(request.Id, request.ServiceName, request.ServiceType, request.ServiceDescription,
                request.ServiceFrequency, request.Comments, request.ServiceProviderEmail, request.ServiceProviderName, request.PropertyManagerEmail, request.PropertyManagerName);
            return result;  

        }
        public async Task<Result> GatherUserServices(string username)
        {
            Result result = await _userServiceDAO.GetUserService(username, username);
            return result;
        }

        public async Task<Result> RateService(Guid id, int rate)
        {
            Result result = new Result();
            if (CheckRate(rate))
            {
                result = await _userServiceDAO.RateUserServices(id, rate);
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
