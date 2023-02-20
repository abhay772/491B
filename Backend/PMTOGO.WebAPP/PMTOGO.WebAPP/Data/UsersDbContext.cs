using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using PMTOGO.WebAPP.Models.Entities;

namespace PMTOGO.WebAPP.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }
    }
}
