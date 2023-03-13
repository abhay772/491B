

namespace AA.PMTOGO.Models.Entities;

public class UserService
{
    public Guid ServiceId { get; set; }
    public string PropertyManagerEmail { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceDescription { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string ServiceFrequency { get; set; } = string.Empty;
    public string ServiceProviderEmail { get; set; } = string.Empty;
    public string ServiceProvider { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Rating { get; set; } 

    public UserService() { }

   public UserService(Guid serviceId, string propertyManagerEmail, string serviceName, string serviceDescription, 
       string serviceType, string serviceFrequency, string serviceProviderEmail, string serviceProvider, string status, int rating)
    {
        ServiceId = serviceId;
        PropertyManagerEmail = propertyManagerEmail;
        ServiceName = serviceName;
        ServiceDescription = serviceDescription;
        ServiceType = serviceType;
        ServiceFrequency = serviceFrequency;
        ServiceProviderEmail = serviceProviderEmail;
        ServiceProvider = serviceProvider;
        Status = status;
        Rating = rating;
    }
}
