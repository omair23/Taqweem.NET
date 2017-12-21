using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Taqweem.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Taqweem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Models.TimeZone> TimeZones { get; set; }

        public DbSet<Masjid> Masjids { get; set; }

        public DbSet<SalaahTime> SalaahTimes { get; set; }

        public DbSet<Notice> Notices { get; set; }

        public DbSet<Configuration> Configurations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Models.TimeZone>().ToTable("TimeZone");

            builder.Entity<Masjid>().ToTable("Masjid");

            builder.Entity<Masjid>().Property(u => u.UID).UseSqlServerIdentityColumn();

            builder.Entity<Masjid>().Property(p => p.UID)
                .UseSqlServerIdentityColumn();

            builder.Entity<Masjid>().Property(p => p.UID)
                .Metadata.AfterSaveBehavior = PropertySaveBehavior.Ignore;

            builder.Entity<SalaahTime>().ToTable("SalaahTime");

            builder.Entity<Notice>().ToTable("Notice");

            builder.Entity<Configuration>().ToTable("Configuration");

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
