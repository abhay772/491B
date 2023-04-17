using System.ComponentModel.DataAnnotations;



namespace AA.PMTOGO.Models.Entities
{

    public class UserService
    {
        [Key]
        public Guid Id { get; set; }
        [Key]
        public string PropertyManagerEmail { get; set; } = string.Empty;
        public string PropertyManagerName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceFrequency { get; set; } = string.Empty;
        [Required]
        public string ServiceProviderEmail { get; set; } = string.Empty;
        public string ServiceProviderName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public int Rating { get; set; } 
        public int SPRating { get; set; }
        public int PMRating { get; set; }

        public UserService() { }

        public UserService(Guid id, string serviceName, string serviceDescription, string serviceType,
            string serviceFrequency, string serviceProviderEmail, string serviceProviderName, string propertyManagerEmail, string propertyManagerName)
        {
            Id = id;
            ServiceName = serviceName;
            ServiceType = serviceType;
            ServiceDescription = serviceDescription;
            ServiceFrequency = serviceFrequency;
            ServiceProviderEmail = serviceProviderEmail;
            ServiceProviderName = serviceProviderName;
            PropertyManagerEmail = propertyManagerEmail;
            PropertyManagerName = propertyManagerName;

        }
        public UserService(Guid id, string serviceName, string serviceDescription, string serviceType, string serviceFrequency, string serviceProviderEmail, 
            string serviceProviderName, string propertyManagerEmail, string propertyManagerName, string status, int rating)
        {
            Id = id;
            ServiceName = serviceName;
            ServiceType = serviceType;
            ServiceDescription = serviceDescription;
            ServiceFrequency = serviceFrequency;
            ServiceProviderEmail = serviceProviderEmail;
            ServiceProviderName = serviceProviderName;
            PropertyManagerEmail = propertyManagerEmail;
            PropertyManagerName = propertyManagerName;
            Status = status;
            Rating = rating;

        }

    }

}
