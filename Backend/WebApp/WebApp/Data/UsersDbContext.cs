using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using WebApp.Models.Entities;

namespace WebApp.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }
    }
}
