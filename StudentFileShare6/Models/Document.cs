using Microsoft.AspNetCore.Identity;
using StudentFileShare6.Models;
using StudentFileShare6.data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public class Document
{
    public string DocumentID { get; set; }
    public string Name { get; set; }
    public string SchoolID { get; set; }
    public string CourseID { get; set; }
    public int Year { get; set; }
    public int Category { get; set; }
    public string Description { get; set; }
    public float Rating { get; set; }
    public bool Anonymous { get; set; }
    public string UserID { get; set; }
    public string Link { get; set; }   //link to download the document
    public University University { get; set; }
    public Course Course { get; set; }
    public IdentityUser User { get; set; }
}

public class DocumentService
{
    private readonly DocumentContext _context;

    public DocumentService(DocumentContext context)
    {
        _context = context;
    }

    public void GenerateRandomDocumentID(Document document)
    {
        var random = new Random();
        string generatedDocumentID;

        do
        {
            generatedDocumentID = $"{random.Next(1000, 9999)}{document.SchoolID.Substring(0, 2)}{document.CourseID.Substring(0, 2)}";
        } while (!IsDocumentIDUnique(generatedDocumentID));

        document.DocumentID = generatedDocumentID;
    }

    private bool IsDocumentIDUnique(string documentID)
    {
        return !_context.Documents.Any(d => d.DocumentID == documentID);
    }
}





//using Microsoft.AspNetCore.Identity;
//using StudentFileShare6.Models;
//using StudentFileShare6.data;
//public class Document
//{
//    public string DocumentID { get; set; }
//    public string Name { get; set; }
//    public string SchoolID { get; set; }
//    public string CourseID { get; set; }
//    public int Year { get; set; }
//    public int Category { get; set; }
//    public string Description { get; set; }
//    public float Rating { get; set; }
//    public bool Anonymous { get; set; }
//    public string UserID { get; set; }
//    public string Link { get; set; }   // //link to download the document
//    public University University { get; set; }
//    public Course Course { get; set; }
//    public IdentityUser User { get; set; }


//    public void GenerateRandomDocumentID()
//    {
//        // Generate a random unique DocumentID based on the provided information
//        // You can implement your own logic here to ensure uniqueness
//        // Example: concatenate a random number with other fields

//        var random = new Random();
//        string generatedDocumentID;

//        do
//        {
//            generatedDocumentID = $"{random.Next(1000, 9999)}{SchoolID.Substring(0, 2)}{CourseID.Substring(0, 2)}";
//        } while (!IsDocumentIDUnique(generatedDocumentID));

//        DocumentID = generatedDocumentID;
//    }

//    private bool IsDocumentIDUnique(string documentID)
//    {
//        // Implement your logic to check if the generated DocumentID is unique
//        // Example: query the database to see if any other Document has the same DocumentID

//        using (var context = new DocumentContext())
//        {
//            return !context.Documents.Any(d => d.DocumentID == documentID);
//        }
//    }
//}
