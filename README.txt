
# Project: File Sharing Website (files based on course and university)

see

https://studentfileweb.azurewebsites.net/
//My OSS subscription expire at Oct 24, 2023, means might not be able to see the files uploaded afterwards

## About
This repository contains the codebase for a file sharing website developed using .NET MVC, Razor syntax, HTML, JavaScript, AJAX, external libraries, CSS, and cookie handling. The project includes a user-friendly interface for uploading, searching, viewing, rating, and deleting documents.
This project is for hosting on x64, if host on x86 then pdfium plugin won't work (cannot upload file), need to install Pdfiumviewer x86 in nuget package store in addition.

## Key Features

- **Document Upload:** Users can upload files which are stored in Aliyun OSS and the relevant details are saved in the database. 
- **Document Search:** The search functionality allows users to find documents based on university and course names. The search uses AJAX requests for dynamic results display.
- **Document View:** Each document can be viewed in a PDF viewer implemented with the PDF.js library. Users can like and dislike documents, with their preferences saved in cookies.
- **Document Rating:** Users can rate documents. The rating system uses AJAX to update the rating asynchronously.
- **Data Management:** Entity Framework Core is used for data manipulation, including creating, reading, updating, and deleting records in the database.

For more information on the implementation details and the functionalities of the website, please refer to the comments in the codebase.
