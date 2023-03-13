using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models.Entities
{
    public class ServiceRequest
    {
        
        public Guid ServiceRequestId { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceFrequency { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        [Key]
        public string ServiceProviderEmail { get; set; } = string.Empty; 
        public string PropertyManagerName { get; set; } = string.Empty;
        [Required]
        public string PropertyManagerEmail { get; set; } = string.Empty;

        public ServiceRequest() { }

        public ServiceRequest(Guid requestId, string serviceName, string serviceDescription, string serviceType,
            string serviceFrequency, string comments, string serviceProviderEmail, string propertyManagerName, string propertyManagerEmail)
        {
            ServiceRequestId = requestId;
            ServiceName = serviceName;
            ServiceDescription = serviceDescription;
            ServiceType = serviceType;
            ServiceFrequency = serviceFrequency;
            Comments = comments;
            ServiceProviderEmail = serviceProviderEmail;
            PropertyManagerName = propertyManagerName;
            PropertyManagerEmail = propertyManagerEmail;
        }
    }
}
