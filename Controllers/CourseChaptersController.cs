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
    public class CourseChaptersController : Controller
    {
        private readonly ELearningMVCContext _context;

        public CourseChaptersController(ELearningMVCContext context)
        {
            _context = context;
        }

        // GET: CourseChapters
        public async Task<IActionResult> Index()
        {
            var eLearningMVCContext = _context.CourseChapter.Include(c => c.Course);
            return View(await eLearningMVCContext.ToListAsync());
        }

        // GET: CourseChapters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseChapter = await _context.CourseChapter
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseChapter == null)
            {
                return NotFound();
            }

            return View(courseChapter);
        }

        // GET: CourseChapters/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseBrief");
            return View();
        }

        // POST: CourseChapters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,ChapterTitle,TimeRequiredInSec")] CourseChapter courseChapter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseChapter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseBrief", courseChapter.CourseId);
            return View(courseChapter);
        }

        // GET: CourseChapters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseChapter = await _context.CourseChapter.FindAsync(id);
            if (courseChapter == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseBrief", courseChapter.CourseId);
            return View(courseChapter);
        }

        // POST: CourseChapters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,ChapterTitle,TimeRequiredInSec")] CourseChapter courseChapter)
        {
            if (id != courseChapter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseChapter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseChapterExists(courseChapter.Id))
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
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "CourseBrief", courseChapter.CourseId);
            return View(courseChapter);
        }

        // GET: CourseChapters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseChapter = await _context.CourseChapter
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseChapter == null)
            {
                return NotFound();
            }

            return View(courseChapter);
        }

        // POST: CourseChapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseChapter = await _context.CourseChapter.FindAsync(id);
            _context.CourseChapter.Remove(courseChapter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseChapterExists(int id)
        {
            return _context.CourseChapter.Any(e => e.Id == id);
        }
    }
}
