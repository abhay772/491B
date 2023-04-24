using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA.PMTOGO.Models.Entities
{
    public class Analysis
    {
        public string category { get; set; } = string.Empty;
        public Array data { get; set; }


        public Analysis(string category, Array data)
        {
            this.data = data;
        }
    }
}
