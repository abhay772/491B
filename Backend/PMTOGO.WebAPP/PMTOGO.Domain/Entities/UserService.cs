﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AA.PMTOGO.Models.Entities;

public class UserService
{
    public class UserService
    {
        [Key]
        public Guid ServiceId { get; set; }
        [Key]
        public string PropertyManagerEmail { get; set; } = string.Empty;
        public string PropertyManagerName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceFrequency { get; set; } = string.Empty;
        [Required]
        public string ServiceProviderEmail { get; set; } = string.Empty;
        public string ServiceProvider { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Rating { get; set; }
        [Key]
        public Guid ServiceId { get; set; }
        [Key]
        public string PropertyManagerEmail { get; set; } = string.Empty;
        public string PropertyManagerName { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceFrequency { get; set; } = string.Empty;
        [Required]
        public string ServiceProviderEmail { get; set; } = string.Empty;
        public string ServiceProvider { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int Rating { get; set; }

        public UserService() { }

        public UserService(Guid serviceId, string serviceName, string serviceDescription, string serviceType,
            string serviceFrequency, string serviceProviderEmail, string serviceProvider, string propertyManagerEmail, string propertyManagerName)
        {
            ServiceId = serviceId;
            ServiceName = serviceName;
            ServiceType = serviceType;
            ServiceDescription = serviceDescription;
            ServiceFrequency = serviceFrequency;
            ServiceProviderEmail = serviceProviderEmail;
            ServiceProvider = serviceProvider;
            PropertyManagerEmail = propertyManagerEmail;
            PropertyManagerName = propertyManagerName;

        }
        public UserService(Guid serviceId, string serviceName, string serviceDescription, string serviceType, string serviceFrequency, string serviceProviderEmail, string serviceProvider, string propertyManagerEmail, string propertyManagerName, string status, int rating)
    }
}            ServiceName = serviceName;
            ServiceType = serviceType;
            ServiceDescription = serviceDescription;
            ServiceFrequency = serviceFrequency;
            ServiceProviderEmail = serviceProviderEmail;
            ServiceProvider = serviceProvider;
            PropertyManagerEmail = propertyManagerEmail;
            PropertyManagerName = propertyManagerName;
            Status = status;
            Rating = rating;
        }

    }
}
