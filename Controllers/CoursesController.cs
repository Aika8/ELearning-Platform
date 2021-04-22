using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ELearningMVC.Data;
using ELearningMVC.Models;
using System.Dynamic;

namespace ELearningMVC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ELearningMVCContext _context;

        public CoursesController(ELearningMVCContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["TitileSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "title_asc";
            ViewData["TypeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "type_desc" : "";
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var courses = from s in _context.Course
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.CourseTitle.Contains(searchString)
                                       || s.Type.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    courses = courses.OrderByDescending(s => s.CourseTitle);
                    break;
                case "title_asc":
                    courses = courses.OrderBy(s => s.CourseTitle);
                    break;
                case "type_desc":
                    courses = courses.OrderByDescending(s => s.Type);
                    break;
                default:
                    courses = courses.OrderBy(s => s.Type);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Course>.CreateAsync(courses.Include(a => a.Language).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Language)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["LanguageId"] = new SelectList(_context.Language, "Id", "Id");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseTitle,CourseBrief,Teacher,Type,LanguageId,ImgUrl")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LanguageId"] = new SelectList(_context.Language, "Id", "Id", course.LanguageId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["LanguageId"] = new SelectList(_context.Language, "Id", "Id", course.LanguageId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseTitle,CourseBrief,Teacher,Type,LanguageId,ImgUrl")] Course course)
        {
            if (id != course.Id)
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
                    if (!CourseExists(course.Id))
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
            ViewData["LanguageId"] = new SelectList(_context.Language, "Id", "Id", course.LanguageId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Language)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
