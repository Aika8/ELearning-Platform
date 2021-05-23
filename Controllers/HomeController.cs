using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ELearningMVC.Data;
using ELearningMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Dynamic;
using System.ComponentModel;

namespace ELearningMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ELearningMVCContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public HomeController(ILogger<HomeController> logger, ELearningMVCContext context, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index(String search)
        {
            var course = from s in _context.Course
                                       select s;

            if (search != null)
            {
                course = course.Where(c => c.CourseTitle.StartsWith(search)).Include(a => a.Language);
                
            }
            else
            {

                if (HttpContext.Session.GetString("lang") == null || HttpContext.Session.GetString("lang") == "all" || HttpContext.Session.GetString("lang") == "")
                {
                    course = course.Include(c => c.Language);
                }
                else
                {
                    string lang = HttpContext.Session.GetString("lang");
                    course = course.Where(c => c.Language.Code == lang).Include(c => c.Language);
                }
            }

            ViewData["Courses"] = course.ToList();
            ViewData["Lang"] = _context.Language.ToList();
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult Cook(String lang)
        {

            HttpContext.Session.SetString("lang", lang);



            return RedirectToAction("Index");
        }

        public ActionResult AutocompleteSearch(string term)
        {
            var courses = _context.Course.Where(a => a.CourseTitle.Contains(term))
                            .Select(a => new { value = a.CourseTitle })
                            .Distinct();

            return Json(courses);
        }

        [HttpPost]
        public JsonResult IsUserNameAvailable(string FullName)
        {
            return Json(!_context.Course.Any(x => x.Teacher == FullName),
                                                 new Newtonsoft.Json.JsonSerializerSettings());
        }

        public ActionResult CourseDescription(int id)
        {
            var enrollments = false;


            Course c = _context.Course.Include(m => m.CourseChapter).ThenInclude(mi => mi.CourseChapterContent).Include(s=>s.Students).FirstOrDefault(e => e.Id == id);

            string Name = HttpContext.User.Identity.Name;
            if (_context.Student.Where(i => i.UserName == Name).Any())
            {
                Student user = (Student)_context.Users.Where(i => i.UserName == Name).FirstOrDefault();
                enrollments = c.Students.Contains(user);
            }
          


            if (enrollments)
            {

                ViewBag.enroll = "yes";
            }
            else
            {
                ViewBag.enroll = "no";
            }


            return PartialView("CourseDescription", c);
        }

        [Obsolete]
        public FileResult DownloadFile(string fileName)
        {

            // application's base path
            string contentRootPath = _hostingEnvironment.ContentRootPath;
            //Build the File Path.
            string path = Path.Combine(contentRootPath, "Files/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        [HttpPost]
        public ActionResult Enroll(int id)
        {
            Course course = _context.Course.Find(id);
            string Name = HttpContext.User.Identity.Name;
            Student user = (Student)_context.Users.Where(i => i.UserName == Name).First();
            user.Courses.Add(course);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
