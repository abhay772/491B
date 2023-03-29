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
        [Key]
        public Guid RequestId { get; set; }
        [Required]
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceFrequency { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        [Key]
        public string ServiceProviderEmail { get; set; } = string.Empty;
        public string ServiceProviderName { get; set; } = string.Empty;
        public string PropertyManagerName { get; set; } = string.Empty;
        [Required]
        public string PropertyManagerEmail { get; set; } = string.Empty;

        public ServiceRequest() { }

        public ServiceRequest(Guid requestId, string serviceName, string serviceType, string serviceDescription, string serviceFrequency,
            string comments, string serviceProviderEmail, string serviceProviderName, string propertyManagerEmail, string propertyManagerName)
        {
            RequestId = requestId;
            ServiceName = serviceName;
            ServiceType = serviceType;
            ServiceDescription = serviceDescription;
            ServiceFrequency = serviceFrequency;
            Comments = comments;
            ServiceProviderEmail = serviceProviderEmail;
            ServiceProviderName = serviceProviderName;
            PropertyManagerEmail = propertyManagerEmail;
            PropertyManagerName = propertyManagerName;

        }
    }
}
