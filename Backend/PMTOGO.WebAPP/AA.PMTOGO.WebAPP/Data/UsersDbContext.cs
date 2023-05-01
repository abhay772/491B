using AA.PMTOGO.DAL;
using Microsoft.EntityFrameworkCore;

namespace AA.PMTOGO.WebAPP.Data
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UsersDAO> Users { get; set; }
    }
}
