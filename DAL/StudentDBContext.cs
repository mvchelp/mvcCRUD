using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcDemo.DAL
{
    public class StudentDBContext : DbContext
    {
        public StudentDBContext()
            : base("StudentDBContext")
        { 
        }

        public DbSet<Students> Students { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Course> Course { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>().
              HasMany<Course>(c => c.Courses).
              WithMany(s => s.Students).
              Map(
               m =>
               {
                   m.MapLeftKey("StudentId");
                   m.MapRightKey("CourseId");
                   m.ToTable("StudentCourses");
               });
        }
    }
}