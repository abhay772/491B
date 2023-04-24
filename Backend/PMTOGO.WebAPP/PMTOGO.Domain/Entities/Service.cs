using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models.Entities
{
    public class Service
    {
        [Key]
        public Guid Id { get; set; }
        [Key]
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
        
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceProvider { get; set; } = string.Empty;
        [Key]
        public string ServiceProviderEmail { get; set; } = string.Empty;

        public double ServicePrice { get; set; }
        
        public Service() { }

        public Service(Guid id, string name, string type, string description, string serviceProvider, string providerEmail, double servicePrice )
        {
            Id = id;
            ServiceName = name;
            ServiceDescription = description;
            ServiceType = type;
            ServiceProvider = serviceProvider;
            ServiceProviderEmail = providerEmail;
            ServicePrice = servicePrice;
        }

    }
}
