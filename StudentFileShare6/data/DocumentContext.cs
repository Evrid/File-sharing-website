

using Microsoft.EntityFrameworkCore;

namespace StudentFileShare6.data
{
    public class DocumentContext : DbContext
    {
        public DocumentContext(DbContextOptions<DocumentContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>()
                .HasKey(d => d.DocumentID);

            modelBuilder.Entity<Document>()
                .Property(d => d.DocumentID)
                .IsRequired()
                .HasMaxLength(128);

            modelBuilder.Entity<Document>()
                .Property(d => d.Name)
                .HasMaxLength(128);

            modelBuilder.Entity<Document>()
                .Property(d => d.Description)
                .HasMaxLength(128);

            modelBuilder.Entity<Document>()
                .Property(d => d.Rating)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Document>()
                .HasOne(d => d.University)
                .WithMany(u => u.Documents)
                .HasForeignKey(d => d.SchoolID);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.Course)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CourseID);

            modelBuilder.Entity<Document>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.UserID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}



//using Microsoft.EntityFrameworkCore;


//namespace StudentFileShare6.data
//{
//    public class DocumentContext : DbContext
//    {

//        public DocumentContext(DbContextOptions options) : base(options)
//        //will establish a connection to the class
//        {

//        }

//        public DbSet<Document> Documents { get; set; }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Document>()
//                .HasKey(d => d.DocumentID);

//            modelBuilder.Entity<Document>()
//                .Property(d => d.DocumentID)
//                .IsRequired()
//                .HasMaxLength(128);

//            modelBuilder.Entity<Document>()
//                .Property(d => d.Name)
//                .HasMaxLength(128);

//            modelBuilder.Entity<Document>()
//                .Property(d => d.Description)
//                .HasMaxLength(128);

//            modelBuilder.Entity<Document>()
//                .Property(d => d.Rating)
//                .HasColumnType("decimal(18, 2)");

//            modelBuilder.Entity<Document>()
//                .HasOne(d => d.University)
//                .WithMany(u => u.Documents)
//                .HasForeignKey(d => d.SchoolID);

//            modelBuilder.Entity<Document>()
//                .HasOne(d => d.Course)
//                .WithMany(c => c.Documents)
//                .HasForeignKey(d => d.CourseID);

//            modelBuilder.Entity<Document>()
//                .HasOne(d => d.User)
//                .WithMany()
//                .HasForeignKey(d => d.UserID)
//                .OnDelete(DeleteBehavior.Restrict);
//        }
//    }

//}
