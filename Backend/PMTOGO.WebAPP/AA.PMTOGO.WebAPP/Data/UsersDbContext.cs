using AA.PMTOGO.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AA.PMTOGO.WebAPP.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }
        //public DbSet<UsersDAO> Users { get; set; }

        public DbSet<User> User { get; set; } = null!;
    }
}
