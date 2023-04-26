using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models.Entities
{
    public class DIYObject
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MemoryStream Video { get; set; }
    }
}
