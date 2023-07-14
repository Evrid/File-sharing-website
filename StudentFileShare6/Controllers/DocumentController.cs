﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentFileShare6.data;
using StudentFileShare6.Models;
//using AliyunOss;
using Aliyun.OSS;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net;
using Aliyun.OSS.Common;
using System.Security.AccessControl;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.SignalR;
using StudentFileShare6.Hubs;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Web;
using PdfiumViewer;
using System.Drawing;
using System.Drawing.Imaging;
using RestSharp;

namespace StudentFileShare6.Controllers
{
    public class DocumentController : Controller
    {
        private readonly DocumentContext _context;
        private readonly IHubContext<ProgressHub> _hubContext;
        private readonly UniversityContext _universityContext;
        private readonly CourseContext _courseContext;


        public DocumentController(DocumentContext context, IHubContext<ProgressHub> hubContext, CourseContext courseContext, UniversityContext universityContext)
        {
            _context = context;
            _hubContext = hubContext;

            _courseContext = courseContext;
            _universityContext = universityContext;

        }

        // GET: Document
        public async Task<IActionResult> Index()
        {
            var documentContext = _context.Document.Include(d => d.Course).Include(d => d.University);
            var documents = await documentContext.ToListAsync();

            // Fetch the base64-encoded image for each document and add it to the ViewData dictionary
            foreach (var document in documents)
            {
                string imageUrl = document.FirstPageImageLink;
                ViewData[document.DocumentID.ToString()] = imageUrl;
            }

            return View(documents);
        }
        // GET: Document/UserUploadedDocumentsPage
        public async Task<IActionResult> UserUploadedDocumentsPage()
        {
            var documentContext = _context.Document.Include(d => d.Course).Include(d => d.University);
            var documents = await documentContext.ToListAsync();

            // Fetch the base64-encoded image for each document and add it to the ViewData dictionary
            foreach (var document in documents)
            {
                string imageUrl = document.FirstPageImageLink;
                ViewData[document.DocumentID.ToString()] = imageUrl;
            }

            return View(documents);
        }

        // GET: Document/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.Course)
                .Include(d => d.University)
                .FirstOrDefaultAsync(m => m.DocumentID == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Document/Create
        public IActionResult Create()
        {
            var document = new Document();
            return View(document);
        }

        [HttpGet]
        public IActionResult GetUniversities(string searchText)
        {
            //the GetUniversities function is not directly called in the Create.cshtml file. Instead, it is called asynchronously from JavaScript code using the fetch API
            List<University> universities = _universityContext.Universities.ToList();
            var matchingUniversities = universities
                 .Where(u => ((u.Name).ToLower()).StartsWith(searchText.ToLower()))
                 //  .Where(u => ((u.Name).ToLower()).Contains(searchText.ToLower()))
                 //  we want if user enter small case it can match what we got in the database
                 .Select(u => new { Name = u.Name, SchoolID = u.SchoolID, Location = u.Location })
                 .Take(10)
                 .ToList();
            //When using var, we declare implicit type variables, the compiler infers the variable’s type from its value automatically
            return Json(matchingUniversities);
        }

        [HttpGet]
        public IActionResult GetCourses(string searchText, string SelectedSchoolID)
        {
            //the GetUniversities function is not directly called in the Create.cshtml file. Instead, it is called asynchronously from JavaScript code using the fetch API
            List<Course> courses = _courseContext.Course.ToList();
            var matchingCourses = courses
                  .Where(u => ((u.CourseName).ToLower()).StartsWith(searchText.ToLower()) && u.SchoolID == SelectedSchoolID)
                  //.Where(u => ((u.CourseName).ToLower()).Contains(searchText.ToLower()) && u.SchoolID == SelectedSchoolID)
                  .Select(u => new { Name = u.CourseName, CourseID = u.CourseID })
                  .Take(10)
                  .ToList();
            //When using var, we declare implicit type variables, the compiler infers the variable’s type from its value automatically
            return Json(matchingCourses);
        }





        [HttpGet]
        public IActionResult ifUniversityExist(string givenName, string givenLocation)
        //To prevent the user from submitting the form if the university name already exists in the database
        {
            List<University> universities = _universityContext.Universities.ToList();
            var matchingUniversities = universities
                 .Where(u => u.Name.ToLower() == givenName.ToLower() && u.Location.ToLower() == givenLocation.ToLower())
                 .Select(u => new { Name = u.Name, SchoolID = u.SchoolID, Location = u.Location })
                 .Take(10)
                 .ToList();

            return Json(matchingUniversities);
        }


        [HttpGet]
        public IActionResult ifCourseExist(string givenCourse, string SelectedSchoolID)
        {
            //the GetUniversities function is not directly called in the Create.cshtml file. Instead, it is called asynchronously from JavaScript code using the fetch API
            List<Course> courses = _courseContext.Course.ToList();
            var matchingCourses = courses
                  .Where(u => ((u.CourseName).ToLower()).StartsWith(givenCourse.ToLower()) && u.SchoolID.ToLower() == SelectedSchoolID.ToLower())
                  //.Where(u => ((u.CourseName).ToLower()).Contains(searchText.ToLower()) && u.SchoolID == SelectedSchoolID)
                  .Select(u => new { Name = u.CourseName, CourseID = u.CourseID })
                  .Take(10)
                  .ToList();
            //When using var, we declare implicit type variables, the compiler infers the variable’s type from its value automatically
            return Json(matchingCourses);
        }

        // POST: Document/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DocumentID,Name,SchoolID,CourseID,Year,Category,Description,Anonymous,UserID,Link")] Document document, IFormFile file)
        {


            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                    }
                }

                return View(document);
            }


            if (ModelState.IsValid)
            {


                document.GenerateRandomDocumentID(_context);

                if (file != null && file.Length > 0)
                {



                    var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                    //I have put the connection details in appsettings.json
                    var accessKeyId = configuration.GetSection("CloudOSSConnection")["accessKeyId"];
                    var accessKeySecret = configuration.GetSection("CloudOSSConnection")["accessKeySecret"];
                    var endpoint = configuration.GetSection("CloudOSSConnection")["endpoint"];
                    var bucketName = configuration.GetSection("CloudOSSConnection")["bucketName"];
                    var bucketForFirstPagePdf = configuration.GetSection("CloudOSSConnection")["bucketNameForFirstPagePdf"];
                    // because for OSS, if you upload an object that has the same name as an existing object, the existing object is overwritten by the uploaded object
                    //another method see https://www.alibabacloud.com/help/en/object-storage-service/latest/disable-overwrite-for-objects-with-the-same-name-3
                    string fileName = Path.GetFileName(file.FileName);  // assuming fileName is "example.txt"
                    string documentIDTemp = document.DocumentID;
                    // Find the position of the dot in fileName
                    int dotIndex = fileName.IndexOf(".");
                    //to add document ID in front of the dot to let files have different names
                    if (dotIndex != -1)
                    {
                        // Insert documentID in front of the dot
                        fileName = fileName.Insert(dotIndex, documentIDTemp);
                    }


                    var fileStream = file.OpenReadStream();



                    document.FileUploadProgresses = new List<FileUploadProgress>
                    //for a progress bar show upload percenatge
                      {
                   new FileUploadProgress { FileID =document.DocumentID, ProgressPercentage = 0 }
                        };






                    var ossClient = new OssClient(endpoint, accessKeyId, accessKeySecret);




                    //---------------------------below for the link to the full file

                    //

                    var putObjectRequest = new PutObjectRequest(bucketName, fileName, fileStream);
                    putObjectRequest.StreamTransferProgress += (object sender, StreamTransferProgressArgs args) => streamProgressCallbackAsync(sender, args, document, document.DocumentID, _hubContext);


                    ossClient.PutObject(putObjectRequest);



                    var request = new GeneratePresignedUriRequest(bucketName, fileName, SignHttpMethod.Get);
                    request.Expiration = DateTime.UtcNow.AddYears(100); // Set expiration to a distant future date, practically making it non-expiring

                    var objectUrl = ossClient.GeneratePresignedUri(request); // Generate the pre-signed URL


                    document.Link = objectUrl.ToString();

                    //---------------------------above for the link to the full file

                    //---------------------------below for the link to first page of pdf

                    string base64String = StoreCompressedScreenshotInDatabase(file);  //return a base64 string that we can conver to image
                                                                                      //  var putObjectRequestForFirstPagePdf = new PutObjectRequest(bucketForFirstPagePdf, fileName, fileStream);

                    // Convert base64 string to byte array
                    byte[] byteArray = Convert.FromBase64String(base64String);

                    ossClient.PutObject(bucketForFirstPagePdf, document.DocumentID, new MemoryStream(byteArray));


                    var requestForPdfFirstPage = new GeneratePresignedUriRequest(bucketForFirstPagePdf, document.DocumentID, SignHttpMethod.Get);
                    requestForPdfFirstPage.Expiration = DateTime.UtcNow.AddYears(100); // Set expiration to a distant future date, practically making it non-expiring

                    var objectUrlPdfFirstPage = ossClient.GeneratePresignedUri(requestForPdfFirstPage); // Generate the pre-signed URL


                    document.FirstPageImageLink = objectUrlPdfFirstPage.ToString();




                    //---------------------------above for the link to first page of pdf

                }


                document.LikeNumber = 0;
                document.DislikeNumber = 0;
                document.Rating = null;
               // document.Rating = 0;
                _context.Add(document);
                await _context.SaveChangesAsync();
                //  return RedirectToAction(nameof(Index));    

                // return RedirectToAction("Index", "Home"); //redirect to "index" action of "home" controller

                return RedirectToAction("DocumentCreateSuccess", "Document");   //redirect to "DocumentCreateSuccess" action of "Document" controller
            }
           // ViewData["CourseID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseID", document.CourseID);
           // ViewData["SchoolID"] = new SelectList(_context.Set<University>(), "SchoolID", "SchoolID", document.SchoolID);
            return View(document);
        }

        public async Task<IActionResult> streamProgressCallbackAsync(object sender, StreamTransferProgressArgs args, Document document, string fileID, IHubContext<ProgressHub> progressHubContext)
        //to display upload progress dynamically so we need use async
        {
            var progressPercentage = args.TransferredBytes * 100 / args.TotalBytes;

            var fileUploadProgress = document.FileUploadProgresses.FirstOrDefault(f => f.FileID == fileID);

            if (fileUploadProgress != null)
            {
                // Update the progress percentage
                fileUploadProgress.ProgressPercentage = (int)progressPercentage;
                // Broadcast the progress update to connected clients

                //We we need to activate progressHub here because we are passing fileID and progressPercentage 
            }

            // Log the progress update
            System.Diagnostics.Debug.WriteLine($"Progress: {progressPercentage}%, TotalBytes: {args.TotalBytes}, TransferredBytes: {args.TransferredBytes}");

            await _hubContext.Clients.All.SendAsync("UpdateFileProgressIdentifier", fileID, progressPercentage);
            return Ok();
        }

        private string StoreCompressedScreenshotInDatabase(IFormFile file)
        //take a screenshot of the first page of a pdf document after user uploaded as a base64-encoded image string and store in Aliyun OSS
        {
            using (var pdfDocument = PdfDocument.Load(file.OpenReadStream()))
            {
                using (var imageStream = new MemoryStream())
                {
                    var pdfPage = pdfDocument.Render(0, 300, 300, false); // Adjust width and height as needed

                    EncoderParameters encoderParams = new EncoderParameters(1);
                    encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);

                    ImageCodecInfo jpegCodec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

                    pdfPage.Save(imageStream, jpegCodec, encoderParams);

                    byte[] imageBytes = imageStream.ToArray();
                    string base64Image = Convert.ToBase64String(imageBytes);

                    return base64Image;    //works well, copy the value and remove the "" then we can convert to image online 
                    //return a base64 string that we can conver to image for first page of pdf
                }
            }
        }





        // GET: Document/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            ViewData["CourseID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseID", document.CourseID);
            ViewData["SchoolID"] = new SelectList(_context.Set<University>(), "SchoolID", "SchoolID", document.SchoolID);
            return View(document);
        }

        // POST: Document/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DocumentID,Name,SchoolID,CourseID,Year,Category,Description,Rating,Anonymous,UserID,Link")] Document document)
        {
            if (id != document.DocumentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.DocumentID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseID"] = new SelectList(_context.Set<Course>(), "CourseID", "CourseID", document.CourseID);
            ViewData["SchoolID"] = new SelectList(_context.Set<University>(), "SchoolID", "SchoolID", document.SchoolID);
            return View(document);
        }

        // GET: Document/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Document == null)
            {
                return NotFound();
            }

            var document = await _context.Document
                .Include(d => d.Course)
                .Include(d => d.University)
                .FirstOrDefaultAsync(m => m.DocumentID == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Document/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Document == null)
            {
                return Problem("Entity set 'DocumentContext.Document'  is null.");
            }
            var document = await _context.Document.FindAsync(id);
            if (document != null)
            {
                _context.Document.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(string id)
        {
            return (_context.Document?.Any(e => e.DocumentID == id)).GetValueOrDefault();
        }




        public async Task<IActionResult> DocView(string schoolName, string courseName, string documentName, string documentID)
        {
            //to have own page for each file with a website template showing schoolName, courseName, documentName and documentID
            //example link: https://localhost:7192/Document/docview/ABCSchool/Mathematics/SampleDocument/456
            //before adding MapControllerRoute in Program.cs used https://localhost:7192/Document/DocView?schoolName=ABCSchool&courseName=Mathematics&documentName=SampleDocument&documentID=456

            var model = new DocumentViewModel
            {
                SchoolName = schoolName,
                CourseName = courseName,
                DocumentName = documentName,
                DocumentID = documentID
            };


            var theDocument = await _context.Document.FirstOrDefaultAsync(d => d.DocumentID == documentID); // Assuming "Documents" is the DbSet for your "Document" SQL table
                                                                                                            //I want to find item in the "Document" SQL table that column "DocumentID" equals the "documentID" we have, then set "Link" of that "DocumentID" to "Link" of  this "model"
            if (theDocument != null)
            {
                var httpsUrl = theDocument.Link.Replace("http://", "https://");
                //because the link generated is http, and to display on website we need use https, so we do replace here
                model.Link = httpsUrl;
                model.LikeNumber = theDocument.LikeNumber;
                model.DislikeNumber = theDocument.DislikeNumber;
                model.Rating = theDocument.Rating;
            }


            return View(model);
        }

        public ActionResult RedirectToUrl(string href)
        {
            //use a string of link that it can redirect to comtains schoolName, courseName, documentName, and documentID
            return Redirect(href);
        }

        public async Task<IActionResult> DocumentCreateSuccess()
        {
            //after upload complete we show this
            return View();
        }

    }
}
