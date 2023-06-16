using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentFileShare6.data;
using System.Text.RegularExpressions;

namespace StudentFileShare6.Controllers
{
    public class UniversityController : Controller
    {
        private readonly UniversityContext _context;

        public UniversityController(UniversityContext context)
        {
            _context = context;
        }

        // GET: University
        public async Task<IActionResult> Index()
        {
              return _context.Universities != null ? 
                          View(await _context.Universities.ToListAsync()) :
                          Problem("Entity set 'UniversityContext.Universities'  is null.");
        }

        // GET: University/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Universities == null)
            {
                return NotFound();
            }

            var university = await _context.Universities
                .FirstOrDefaultAsync(m => m.SchoolID == id);
            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }

        // GET: University/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: University/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolID","Name","Location")] University university)
        {
            //For the ModelState.IsValid property to be true in this case, the University object passed to the Create method must contain the properties specified in the [Bind] attribute: "SchoolID", "Name", and "Location". If any of these properties are missing or their values fail validation, ModelState.IsValid will be false.
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

                return View(university);
            }
            
            
            if (ModelState.IsValid)
            //The ModelState.IsValid property in ASP.NET MVC is used to determine if the model passed to the action method is valid based on the validation rules defined in the model class. When ModelState.IsValid is false, it means that there are validation errors present.  
            //disabled for now because strange error, later use client side valiation instead (on cshtml file)
              {

            university.GenerateRandomSchoolID(_context);   //generate a random university ID
            _context.Add(university);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

               }
            return View(university);
        }




        // GET: University/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Universities == null)
            {
                return NotFound();
            }

            var university = await _context.Universities.FindAsync(id);
            if (university == null)
            {
                return NotFound();
            }
            return View(university);
        }

        // POST: University/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("SchoolID,Name,Location")] University university)
        {
            if (id != university.SchoolID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(university);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UniversityExists(university.SchoolID))
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
            return View(university);
        }

        // GET: University/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Universities == null)
            {
                return NotFound();
            }

            var university = await _context.Universities
                .FirstOrDefaultAsync(m => m.SchoolID == id);
            if (university == null)
            {
                return NotFound();
            }

            return View(university);
        }

        // POST: University/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Universities == null)
            {
                return Problem("Entity set 'UniversityContext.Universities'  is null.");
            }
            var university = await _context.Universities.FindAsync(id);
            if (university != null)
            {
                _context.Universities.Remove(university);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UniversityExists(string id)
        {
          return (_context.Universities?.Any(e => e.SchoolID == id)).GetValueOrDefault();
        }
    }
}
