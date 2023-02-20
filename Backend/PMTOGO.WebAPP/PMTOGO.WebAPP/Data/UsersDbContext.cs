using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using PMTOGO.Domain.Entities;

namespace PMTOGO.WebAPP.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
