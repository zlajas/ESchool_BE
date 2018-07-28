using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace School.Models
{
    public class DataAccessContext : DbContext
    {
       /* public DataAccessContext() : base("ESchoolConnection")
        {
            Database.SetInitializer(new AuthContextSeedInitializer());
        }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentToSubject> StudentAttendsSubjects { get; set; }
        public DbSet<TeacherToSubject> TeacherTeachesSubjects { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Students");
            });

            modelBuilder.Entity<Parent>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Parents");
            });

            modelBuilder.Entity<Teacher>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Teachers");
            });
        }*/
    }
}