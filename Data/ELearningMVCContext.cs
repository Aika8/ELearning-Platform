using Microsoft.EntityFrameworkCore;
using ELearningMVC.Models;

namespace ELearningMVC.Data
{
    public class ELearningMVCContext : DbContext
    {
        public ELearningMVCContext(DbContextOptions<ELearningMVCContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseChapter> CourseChapter { get; set; }
        public DbSet<CourseChapterContent> CourseChapterContent { get; set; }
        public DbSet<Language> Language {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                    .HasMany(c => c.Students)
                    .WithMany(s => s.Courses)
                    .UsingEntity(j => j.ToTable("Enrollments"));
        }
    }
}
