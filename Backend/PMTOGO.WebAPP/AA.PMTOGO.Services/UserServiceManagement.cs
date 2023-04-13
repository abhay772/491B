using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System.Security.Principal;

namespace AA.PMTOGO.Services
{
    //input validation, error handling , logging
    public class UserServiceManagement : IUserServiceManagement
    {
        UserServiceDAO _userServiceDAO = new UserServiceDAO();
        ServiceRequestManagement _request = new ServiceRequestManagement();
        ServiceDAO _serviceDAO = new ServiceDAO();
        UsersDAO _authNDAO = new UsersDAO();

        private async Task<string> GetUserInfo (string username)
        {
            //get property manager name
            Result res = await _authNDAO.GetUser(username);
            User user = (User)res.Payload!;


            //get user info
            string firstName = user!.FirstName;
            string lastName = user!.LastName;
            string propertyManagerName = firstName + " " + lastName;

            return propertyManagerName;
        }
        public async Task<Result> CreateRequest(ServiceRequest service, string username)
        {
            //need property manager info
            string propertyManagerName = await GetUserInfo(username);

            Guid serviceRequestId = Guid.NewGuid();
            
            ServiceRequest request = new ServiceRequest(serviceRequestId,service.RequestType, service.ServiceName, service.ServiceType, service.ServiceDescription, service.ServiceFrequency, service.Comments, service.ServiceProviderName, service.ServiceProviderEmail,
                username, propertyManagerName);

            Result result = await _request.AddRequest(request);
            return result;
        }

        public async Task<Result> GatherUserServices(string username, string role)
        {
            Result result = new Result();
            try
            {
                if (role == "Service Provider")
                {
                    string query = "SELECT * FROM UserServices WHERE ServiceProviderEmail = @ServiceProviderEmail";
                    result = await _userServiceDAO.GetUserService(query, username);
                    return result;

                }
                if(role == "Property Manager")
                {
                    string query = "SELECT * FROM UserServices WHERE PropertyManagerEmail = @PropertyManagerEmail";
                    result = await _userServiceDAO.GetUserService(query, username);
                    return result;

                }
                result.IsSuccessful = false;
                result.ErrorMessage = "User role is not valid";
                return result;

            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Could not be get user services";
                return result;
            }
        }

        public async Task<Result> Rate(Guid id, int rate, string role)
        {
            Result result = new Result();
            try
            {
                if (CheckRate(rate))
                {
                    if(role == "Service Provider")
                    {
                        string query = "UPDATE UserServices SET SPRating = @Rating WHERE Id = @ID";
                        result = await _userServiceDAO.UpdateServiceRate(id, rate, query);
                        return result;
                    }
                    if (role == "Property Manager")
                    {
                        string query = "UPDATE UserServices SET PMRating = @Rating WHERE Id = @ID";
                        result = await _userServiceDAO.UpdateServiceRate(id, rate, query);
                        return result;
                    }
                }
                result.IsSuccessful = false;
                result.ErrorMessage = "Rate is out of range. Enter a number between 1-5.";
                return result; 
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Rate could not be updated";
                return result;
            }
        }
        public bool CheckRate(int rate)
        {
            if (rate > 5)
            {
                return false;
            }
            return true;
        }
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

        public async Task<Result> FrequnecyChange(Guid id, string frequency)
        {
            Result result = new Result();
            try
            {
                result = await _userServiceDAO.UpdateServiceFrequency(id, frequency);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Frequency could not be updated";
                return result;

            }
        }

        public async Task<Result> CancelUserService(Guid id)
        {
            Result result = new Result();
            try
            {
                result = await _userServiceDAO.DeleteUserService(id);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "User Service Cancellation Not Successsful";
                return result;

            }
        }
    }
}
