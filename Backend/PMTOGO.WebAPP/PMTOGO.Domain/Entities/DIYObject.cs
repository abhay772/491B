using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models.Entities
{
    public class DIYObject
    {
        public string ID { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; }= string.Empty;
        public string Description { get; set; } = string.Empty;
        public MemoryStream? Video { get; set; }
    }
}
