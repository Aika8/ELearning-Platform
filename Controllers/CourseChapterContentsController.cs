using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ELearningMVC.Data;
using ELearningMVC.Models;

namespace ELearningMVC.Controllers
{
    public class CourseChapterContentsController : Controller
    {
        private readonly ELearningMVCContext _context;

        public CourseChapterContentsController(ELearningMVCContext context)
        {
            _context = context;
        }

        // GET: CourseChapterContents
        public async Task<IActionResult> Index(string sortOrder,
        string currentFilter,
        string searchString,
        int? pageNumber)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
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

            var contents = from s in _context.CourseChapterContent
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                contents = contents.Where(s => s.ContentName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    contents = contents.OrderByDescending(s => s.ContentName);
                    break;
                default:
                    contents = contents.OrderBy(s => s.ContentName);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<CourseChapterContent>.CreateAsync(contents.Include(a => a.CourseChapter).AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: CourseChapterContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseChapterContent = await _context.CourseChapterContent
                .Include(c => c.CourseChapter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseChapterContent == null)
            {
                return NotFound();
            }

            return View(courseChapterContent);
        }

        // GET: CourseChapterContents/Create
        public IActionResult Create()
        {
            ViewData["CourseChapterId"] = new SelectList(_context.CourseChapter, "Id", "ChapterTitle");
            return View();
        }

        // POST: CourseChapterContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseChapterId,ContentName,File")] CourseChapterContent courseChapterContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseChapterContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseChapterId"] = new SelectList(_context.CourseChapter, "Id", "ChapterTitle", courseChapterContent.CourseChapterId);
            return View(courseChapterContent);
        }

        // GET: CourseChapterContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseChapterContent = await _context.CourseChapterContent.FindAsync(id);
            if (courseChapterContent == null)
            {
                return NotFound();
            }
            ViewData["CourseChapterId"] = new SelectList(_context.CourseChapter, "Id", "ChapterTitle", courseChapterContent.CourseChapterId);
            return View(courseChapterContent);
        }

        // POST: CourseChapterContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseChapterId,ContentName,File")] CourseChapterContent courseChapterContent)
        {
            if (id != courseChapterContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseChapterContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseChapterContentExists(courseChapterContent.Id))
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
            ViewData["CourseChapterId"] = new SelectList(_context.CourseChapter, "Id", "ChapterTitle", courseChapterContent.CourseChapterId);
            return View(courseChapterContent);
        }

        // GET: CourseChapterContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseChapterContent = await _context.CourseChapterContent
                .Include(c => c.CourseChapter)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseChapterContent == null)
            {
                return NotFound();
            }

            return View(courseChapterContent);
        }

        // POST: CourseChapterContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseChapterContent = await _context.CourseChapterContent.FindAsync(id);
            _context.CourseChapterContent.Remove(courseChapterContent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseChapterContentExists(int id)
        {
            return _context.CourseChapterContent.Any(e => e.Id == id);
        }
    }
}
