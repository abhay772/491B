using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models.Entities
{
    public class Service
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public string Type { get; set; } = string.Empty;
        public string ServiceProvider { get; set; } = string.Empty;
        public string ServiceProviderEmail { get; set; } = string.Empty;
        
        public Service() { }

        public Service(string name, string description, string type, string serviceProvider, string providerEmail)
        {
            Name = name;
            Description = description;
            Type = type;
            ServiceProvider = serviceProvider;
            ServiceProviderEmail = providerEmail;
        }     

    }
}
