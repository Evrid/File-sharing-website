
//using Microsoft.EntityFrameworkCore;
//using StudentFileShare6.Models;

//public class CourseContext : DbContext
//{
//    public DbSet<Course> Courses { get; set; }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        modelBuilder.Entity<Course>()
//            .HasKey(c => c.CourseID);

//        modelBuilder.Entity<Course>()
//            .Property(c => c.CourseID)
//            .IsRequired()
//            .HasMaxLength(128);

//        modelBuilder.Entity<Course>()
//            .Property(c => c.SchoolID)
//            .HasMaxLength(128);

//        modelBuilder.Entity<Course>()
//            .HasOne(c => c.University)
//            .WithMany(u => u.Courses)
//            .HasForeignKey(c => c.SchoolID);
//    }
//}



using Microsoft.EntityFrameworkCore;
using StudentFileShare6.Models;

public class CourseContext : DbContext
{
    public CourseContext(DbContextOptions<CourseContext> options) : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>()
            .HasKey(c => c.CourseID);

        modelBuilder.Entity<Course>()
            .Property(c => c.CourseID)
            .IsRequired()
            .HasMaxLength(128);

        modelBuilder.Entity<Course>()
            .Property(c => c.SchoolID)
            .HasMaxLength(128);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.University)
            .WithMany(u => u.Courses)
            .HasForeignKey(c => c.SchoolID);
    }
}






