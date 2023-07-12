using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentFileShare6.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "universities",
                columns: table => new
                {
                    SchoolID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_universities", x => x.SchoolID);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniversitiesSchoolID = table.Column<string>(type: "nvarchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_Course_universities_UniversitiesSchoolID",
                        column: x => x.UniversitiesSchoolID,
                        principalTable: "universities",
                        principalColumn: "SchoolID");
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    DocumentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<float>(type: "real", nullable: true),
                    Anonymous = table.Column<bool>(type: "bit", nullable: false),
                    UserID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikeNumber = table.Column<int>(type: "int", nullable: true),
                    DislikeNumber = table.Column<int>(type: "int", nullable: true),
                    FirstPageImageLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniversitySchoolID = table.Column<string>(type: "nvarchar(128)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.DocumentID);
                    table.ForeignKey(
                        name: "FK_Document_Course_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Course",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Document_universities_UniversitySchoolID",
                        column: x => x.UniversitySchoolID,
                        principalTable: "universities",
                        principalColumn: "SchoolID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_UniversitiesSchoolID",
                table: "Course",
                column: "UniversitiesSchoolID");

            migrationBuilder.CreateIndex(
                name: "IX_Document_CourseID",
                table: "Document",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_Document_UniversitySchoolID",
                table: "Document",
                column: "UniversitySchoolID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "universities");
        }
    }
}
