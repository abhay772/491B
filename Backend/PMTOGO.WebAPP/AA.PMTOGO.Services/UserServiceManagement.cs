using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using AA.PMTOGO.Services.Interfaces;
using System.Security.Principal;

namespace AA.PMTOGO.Services
{
    //input validation, logging
    public class UserServiceManagement : IUserServiceManagement
    {
        UserServiceDAO _userServiceDAO = new UserServiceDAO();
        ServiceRequestDAO _requestDAO = new ServiceRequestDAO();
        ServiceDAO _serviceDAO = new ServiceDAO();
        UsersDAO _authNDAO = new UsersDAO();

        private async Task<string> GetUserInfo (string username)
        {
            //get property manager name
            try
            {
                Result res = await _authNDAO.GetUser(username);
                User user = (User)res.Payload!;


                //get user info combine first and last name
                string firstName = user!.FirstName;
                string lastName = user!.LastName;
                string propertyManagerName = firstName + " " + lastName;
                return propertyManagerName;
            }
            catch
            {
                Result result = new Result();
                result.ErrorMessage = "Load User Info Unsuccessful. Try Again Later";
                result.IsSuccessful = false;
            }
            return null!;
            
        }
        //change create request to be used with frequency change.
        public async  Task<Result> CreateRequest(Guid id, string type, string frequency)
        {
            Result result = new Result();
            try
            {
                Result findService = await _userServiceDAO.FindUserService(id);
                UserService service = (UserService)findService.Payload!;

                //create new service request
                ServiceRequest newRequest = new ServiceRequest(id, type, service.ServiceName, service.ServiceType, service.ServiceDescription, frequency, type, service.ServiceProviderEmail, service.ServiceProviderName,
                    service.PropertyManagerEmail, service.PropertyManagerName);
                
                result.IsSuccessful = true;
                result.Payload = newRequest;
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Create Request Unsuccessful. Try Again Later";

            }
            return result;
        }
        public async Task<Result> AddRequest(ServiceRequest service, string username)
        {
            Result result = new Result();
            try
            {
                //need property manager info
                string propertyManagerName = await GetUserInfo(username);

                //create id for new services
                Guid serviceRequestId = Guid.NewGuid();

                ServiceRequest request = new ServiceRequest(serviceRequestId, "New Service", service.ServiceName, service.ServiceType, service.ServiceDescription, service.ServiceFrequency, service.Comments, service.ServiceProviderEmail, service.ServiceProviderName,
                    username, propertyManagerName);

                result = await _requestDAO.AddServiceRequest(request);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Add Service Request Unsuccessful. Try Again Later";
            }
            return result;
        }

        public async Task<Result> GatherUserServices(string username, string role)
        {
            Result result = new Result();
            try
            {
                //dont use select *
                //string select = "SELECT Id, ServiceName, ServiceType, ServiceDescription, ServiceFrequency, ServiceProvider, Status";
                if (role == "Service Provider")
                {
                    string query = "SELECT Id, ServiceName, ServiceType, ServiceDescription, ServiceFrequency, ServiceProviderEmail, ServiceProviderName, PropertyManagerEmail, PropertyManagerName, Status, SPRating FROM UserServices WHERE ServiceProviderEmail = @ServiceProviderEmail";
                    result = await _userServiceDAO.GetUserServices(query, username, "SPRating");
                    return result;

                }
                if(role == "Property Manager")
                {
                    string query = "SELECT Id, ServiceName, ServiceType, ServiceDescription, ServiceFrequency, ServiceProviderEmail, ServiceProviderName, PropertyManagerEmail, PropertyManagerName, Status, PMRating FROM UserServices WHERE PropertyManagerEmail = @PropertyManagerEmail";
                    result = await _userServiceDAO.GetUserServices(query, username, "PMRating");
                    return result;

                }
                result.IsSuccessful = false;
                result.ErrorMessage = "User role is not valid";
                return result;

            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load User services Unsuccessful. Try Again Later";
            }
            return result;
        }

        public async Task<Result> Rate(Guid id, int rate, string role)
        {
            Result result = new Result();
            Result rating = new Result();
            try
            {
                if (CheckRate(rate))
                {
                    //different column for user rating
                    if(role == "Service Provider")
                    {
                        string query = "UPDATE UserServices SET SPRating = @Rating WHERE Id = @ID";
                        rating = await _userServiceDAO.UpdateServiceRate(id, rate, query);
                        return rating;
                    }
                    if (role == "Property Manager")
                    {
                        string query = "UPDATE UserServices SET PMRating = @Rating WHERE Id = @ID";
                        rating = await _userServiceDAO.UpdateServiceRate(id, rate, query);
                        return rating;
                    }
                }
                result.IsSuccessful = false;
                result.ErrorMessage = "Rate is out of range. Enter a number between 1-5.";
                return result; 
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Rate Unsuccessful. Try Again Later";  
            }
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
        //get all services from db
        public async Task<Result> GatherServices()
        {
            Result result = new Result();
            try
            {
                result = await _serviceDAO.GetServices();
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Load Services Unsuccessful. Try Again Later";
            }
            return result;
        }

        //create service for service providers
        public async Task<Result> CreateService(Service service)
        {
            Result result = new Result();
            try
            {
               result = await _serviceDAO.AddService(service);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Create Service Unsuccessful. Try Again Later";
            }
            return result;
        }
        //remove service for service providers
        public async Task<Result> RemoveService(Service service)
        {
            Result result = new Result();
            try
            {
                result = await _serviceDAO.DeleteService(service.Id);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Remove Service Unsuccessful. Try Again Later";
            }
            return result;
        }
        

        //new frequency change request is created
        public async Task<Result> RequestFrequencyChange(Guid id, string frequency, string type)
        {
            Result result = new Result();
            try
            {
                //create a frequency change service request using found user service
                Result findRequest = await CreateRequest(id, type, frequency);
                ServiceRequest request = (ServiceRequest)findRequest.Payload!;

            
                //add to service providers request
                result = await _requestDAO.AddServiceRequest(request);

                //if request successful change user service status to pending
                if (result.IsSuccessful) { await ChangeStatus(id, "Pending Frequency Change"); }
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Frequency Request Unsuccessful. Try Again Later";
            }
            return result;
        }

        //new cancellation request is created
        public async Task<Result> CancellationRequest(Guid id, string frequency, string type)
        {
            Result result = new Result();
            try
            {
                //create a frequency change service request using found user service
                Result findRequest = await CreateRequest(id, type, frequency);
                ServiceRequest request = (ServiceRequest)findRequest.Payload!;

                result = await _requestDAO.AddServiceRequest(request);

                //if request successful change user service status to pending
                if (result.IsSuccessful) { await ChangeStatus(id, "Pending Cancellation"); }
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Cancellation Request Unsuccessful. Try Again Later";

            }
            return result;
        }

        public async Task<Result> ChangeStatus(Guid id, string status)
        {
            Result result = new Result();
            try
            {
                result = await _userServiceDAO.UpdateStatus(id, status);
                return result;
            }
            catch
            {
                result.IsSuccessful = false;
                result.ErrorMessage = "Service Status Update Unsuccessful. Try Again Later";                
            }
            return result;
        }

    }
}
