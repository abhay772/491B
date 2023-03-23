using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApp1.Models;

namespace AA.PMTOGO
{
    public class WebApp1Context : DbContext
    {
        public WebApp1Context (DbContextOptions<WebApp1Context> options)
            : base(options)
        {
        }

        public DbSet<Users> Users { get; set; } = default!;
    }
}
