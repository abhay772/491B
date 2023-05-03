using System.ComponentModel.DataAnnotations;

namespace AA.PMTOGO.Models.Entities
{
    public class Service
    {
        [Key]
        public Guid Id { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;

        public string ServiceType { get; set; } = string.Empty;
        public string ServiceProvider { get; set; } = string.Empty;

        public string ServiceProviderEmail { get; set; } = string.Empty;

        public double ServicePrice { get; set; }

        public Service() { }

        public Service(Guid id, string name, string type, string description, string serviceProvider, string providerEmail, double price)
        {
            Id = id;
            ServiceName = name;
            ServiceDescription = description;
            ServiceType = type;
            ServiceProvider = serviceProvider;
            ServiceProviderEmail = providerEmail;
            ServicePrice = price;

        }

        public Service(Guid id, string name, string type, string description, double price)
        {
            this.Id = id;
            this.ServiceName = name;
            this.ServiceType = type;
            this.ServiceDescription = description;
            this.ServicePrice = price;
        }
    }
}
