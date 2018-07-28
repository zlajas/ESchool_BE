using Microsoft.AspNet.Identity.EntityFramework;
using School.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace School.Infrastructure
{
    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext() : base("ESchoolConnection")
        {
            Database.SetInitializer(new AuthContextSeedInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
             modelBuilder.Entity<Student>().ToTable("Students");
             modelBuilder.Entity<Parent>().ToTable("Parents");
             modelBuilder.Entity<Teacher>().ToTable("Teachers");
             modelBuilder.Entity<Admin>().ToTable("Admins");
        }

        public DbSet<Parent> Parents { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentToSubject> StudentAttendsSubjects { get; set; }
        public DbSet<TeacherToSubject> TeacherTeachesSubjects { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
    }


}