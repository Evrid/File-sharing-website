﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentFileShare6.Models;

namespace StudentFileShare6.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseContext _context;

        public CourseController(CourseContext context)
        {
            _context = context;
        }

        // GET: Course
        public async Task<IActionResult> Index()
        {
            var courseContext = _context.Course.Include(c => c.Universities);
            return View(await courseContext.ToListAsync());
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Universities)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Course/Create
        public IActionResult Create()
        {
            //ViewData["SchoolID"] = new SelectList(_context.Set<University>(), "SchoolID", "Name");
            //return View();
            var course = new Course();
            return View(course);

        }

        // POST: Course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,CourseName,SchoolID")] Course course)
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                        //if the ModelState is not valid, it will print the error message in Output's Debug area
                    }
                }

                return View(course);
            }

            if (ModelState.IsValid)
            {
                course.GenerateRandomCourseID(_context);
                _context.Add(course);
                await _context.SaveChangesAsync();
             //   return RedirectToAction(nameof(Index));
                return RedirectToAction("CourseCreateSuccess", "Course");   //redirect to "CourseCreateSuccess" action of "Course" controller
            }
            ViewData["SchoolID"] = new SelectList(_context.Set<University>(), "SchoolID", "SchoolID", course.SchoolID);
            return View(course);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["SchoolID"] = new SelectList(_context.Set<University>(), "SchoolID", "SchoolID", course.SchoolID);
            return View(course);
        }

        // POST: Course/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CourseID,CourseName,SchoolID")] Course course)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
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
            ViewData["SchoolID"] = new SelectList(_context.Set<University>(), "SchoolID", "SchoolID", course.SchoolID);
            return View(course);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Course == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Universities)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Course == null)
            {
                return Problem("Entity set 'CourseContext.Course'  is null.");
            }
            var course = await _context.Course.FindAsync(id);
            if (course != null)
            {
                _context.Course.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(string id)
        {
          return (_context.Course?.Any(e => e.CourseID == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> CourseCreateSuccess()
        {
           //after upload complete we show this
            return View();
        }
    }
}
