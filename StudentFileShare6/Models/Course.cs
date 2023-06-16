

//using StudentFileShare6.data;

//namespace StudentFileShare6.Models
//{
//    public class Course
//    {
//        public string CourseID { get; set; }
//        public string SchoolID { get; set; }
//        public University University { get; set; }

//        public List<Document> Documents { get; set; }

//        public void GenerateRandomCourseID()
//        {
//            // Generate a random unique CourseID based on the SchoolID
//            // You can implement your own logic here to ensure uniqueness
//            // Example: concatenate a random number with the SchoolID

//            var random = new Random();
//            string generatedCourseID;

//            do
//            {
//                generatedCourseID = $"{random.Next(1000, 9999)}{SchoolID}";
//            } while (!IsCourseIDUnique(generatedCourseID));

//            CourseID = generatedCourseID;
//        }

//        private bool IsCourseIDUnique(string courseID)
//        {
//            // Implement your logic to check if the generated CourseID is unique
//            // Example: query the database to see if any other Course has the same CourseID

//            // Assuming you have access to the UniversityContext instance, you can use the following code
//            using (var context = new CourseContext())
//            {
//                return !context.Courses.Any(c => c.CourseID == courseID);
//            }
//        }
//    }


//}





using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;

namespace StudentFileShare6.Models
{
    public class Course
    {
        public string CourseID { get; set; }
        public string SchoolID { get; set; }
        public University University { get; set; }

        public List<Document> Documents { get; set; }

        public void GenerateRandomCourseID(DbContextOptions<CourseContext> options)
        {
            var random = new Random();
            string generatedCourseID;

            do
            {
                generatedCourseID = $"{random.Next(1000, 9999)}{SchoolID}";
            } while (!IsCourseIDUnique(generatedCourseID, options));

            CourseID = generatedCourseID;
        }

        private bool IsCourseIDUnique(string courseID, DbContextOptions<CourseContext> options)
        {
            using (var context = new CourseContext(options))
            {
                return !context.Courses.Any(c => c.CourseID == courseID);
            }
        }
    }
}






