using AA.PMTOGO.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AA.PMTOGO.Infrastructure.Data
{
    public class UsersDbContext: DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {

        }
        //public DbSet<UsersDAO> Users { get; set; }

        public DbSet<User> User { get; set; } = null!;
        public DbSet<Appointment> Appointment { get; set; } = null!;
        public DbSet<Log> Log { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => 
            {
                entity.ToTable("UserAccounts");
            });

            modelBuilder.Entity<User>(entity => 
            {
                entity.ToTable("UserProfiles");
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointment");

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Appointments)
                    .HasForeignKey(e => new { e.Username });
            });

            modelBuilder.Entity<Log>(entity => {
                entity.HasKey(e => e.LogId);

                entity.ToTable("Logs");
            });
        }
    }
}
