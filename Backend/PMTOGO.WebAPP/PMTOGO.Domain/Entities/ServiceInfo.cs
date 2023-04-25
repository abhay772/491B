using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models.Entities
{
    public class ServiceInfo
    {
        public string Id { get; set; } = string.Empty;
        public int rate { get; set; }

        public string frequency { get; set; } = string.Empty;

        public string comments { get; set; } = string.Empty;    
         
    }
}
