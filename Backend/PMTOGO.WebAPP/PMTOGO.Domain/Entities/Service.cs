﻿namespace AA.PMTOGO.Models.Entities
{
    public class Service
    {
        private string name;
        private string type;
        private string description;
        private decimal price;

        public int ID { get; set; }
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceDescription { get; set; } = string.Empty;
    
        public string ServiceType { get; set; } = string.Empty;
        public string ServiceProvider { get; set; } = string.Empty;
  
        public string ServiceProviderEmail { get; set; } = string.Empty;

        public double ServicePrice { get; set; }
        public Service() { }

        public Service(int id, string name, string description, string type, string serviceProvider, string providerEmail, double price)
        {
            ID = id;
            ServiceName = name;
            ServiceDescription = description;
            ServiceType = type;
            ServiceProvider = serviceProvider;
            ServiceProviderEmail = providerEmail;
            ServicePrice = price;
        }

        public Service(int id, string name, string type, string description, decimal price)
        {
            ID = id;
            this.name = name;
            this.type = type;
            this.description = description;
            this.price = price;
        }
    }
}
