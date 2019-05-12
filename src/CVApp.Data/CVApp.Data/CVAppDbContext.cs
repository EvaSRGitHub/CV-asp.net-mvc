﻿using System;
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

        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> Works { get; set; }
        public DbSet<Skill> Skils { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Language> Languages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
