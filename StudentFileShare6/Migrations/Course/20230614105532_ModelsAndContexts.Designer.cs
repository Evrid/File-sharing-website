﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace StudentFileShare6.Migrations.Course
{
    [DbContext(typeof(CourseContext))]
    [Migration("20230614105532_ModelsAndContexts")]
    partial class ModelsAndContexts
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Document", b =>
                {
                    b.Property<string>("DocumentID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Anonymous")
                        .HasColumnType("bit");

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("CourseID")
                        .IsRequired()
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("SchoolID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UniversitySchoolID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("DocumentID");

                    b.HasIndex("CourseID");

                    b.HasIndex("UniversitySchoolID");

                    b.HasIndex("UserID");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("IdentityUser");
                });

            modelBuilder.Entity("StudentFileShare6.Models.Course", b =>
                {
                    b.Property<string>("CourseID")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("SchoolID")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("CourseID");

                    b.HasIndex("SchoolID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("University", b =>
                {
                    b.Property<string>("SchoolID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SchoolID");

                    b.ToTable("University");
                });

            modelBuilder.Entity("Document", b =>
                {
                    b.HasOne("StudentFileShare6.Models.Course", "Course")
                        .WithMany("Documents")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("University", "University")
                        .WithMany("Documents")
                        .HasForeignKey("UniversitySchoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("University");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StudentFileShare6.Models.Course", b =>
                {
                    b.HasOne("University", "University")
                        .WithMany("Courses")
                        .HasForeignKey("SchoolID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("University");
                });

            modelBuilder.Entity("StudentFileShare6.Models.Course", b =>
                {
                    b.Navigation("Documents");
                });

            modelBuilder.Entity("University", b =>
                {
                    b.Navigation("Courses");

                    b.Navigation("Documents");
                });
#pragma warning restore 612, 618
        }
    }
}
