using AA.PMTOGO.DAL;
using AA.PMTOGO.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AA.PMTOGO.WebAPP.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UsersDAO> Users { get; set; }
    }
}
