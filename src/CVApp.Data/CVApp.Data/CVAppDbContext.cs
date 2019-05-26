using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CVApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CVApp.Data
{
    public class CVAppDbContext : IdentityDbContext<CVAppUser>
    {
        public CVAppDbContext(DbContextOptions<CVAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> Works { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Language> Languages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CVAppUser>(entity =>
            {
                entity.HasOne(e => e.Resume).WithOne(r => r.User).HasForeignKey<Resume>(b => b.UserId);
            });

            builder.Entity<CVAppUser>()
            .HasIndex(p => p.UserName)
            .IsUnique();
        }
    }
}
