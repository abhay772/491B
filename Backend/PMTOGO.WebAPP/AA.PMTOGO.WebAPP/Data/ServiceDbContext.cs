﻿using AA.PMTOGO.DAL;
using Microsoft.EntityFrameworkCore;

namespace AA.PMTOGO.WebAPP.Data
{
    public class ServiceDbContext:DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<RequestDAO> Users { get; set; }
    }
}