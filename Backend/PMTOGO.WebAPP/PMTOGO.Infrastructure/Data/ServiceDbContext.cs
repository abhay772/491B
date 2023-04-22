using Microsoft.EntityFrameworkCore;

namespace AA.PMTOGO.Infrastructure.Data
{
    public class ServiceDbContext:DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {

        }
        //public DbSet<RequestDAO> Request { get; set; }
    }
}
