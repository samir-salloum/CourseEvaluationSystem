using Microsoft.AspNetCore.Mvc;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System;

namespace CourseEvaluationSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int? courseId, DateTime? fromDate, DateTime? toDate)
        {
            // Hämta alla kurser till dropdown
            var courses = _context.Courses.ToList();
            ViewBag.Courses = new SelectList(courses, "Id", "Title");

            // Hämta kurser med utvärderingar
            var selectedCourses = _context.Courses
                .Include(c => c.Evaluations)
                .AsQueryable();

            // Filtrera på vald kurs
            if (courseId.HasValue && courseId.Value != 0)
            {
                selectedCourses = selectedCourses.Where(c => c.Id == courseId);
            }

            // Skapa sammanfattning med valda datum
            var summary = selectedCourses
                .Select(c => new CourseSummaryViewModel
                {
                    Course = c.Title,
                    AverageRating = c.Evaluations
                        .Where(e => (!fromDate.HasValue || e.Date >= fromDate)
                                 && (!toDate.HasValue || e.Date <= toDate))
                        .Select(e => e.Rating)
                        .DefaultIfEmpty()
                        .Average(),

                    Comments = c.Evaluations
                        .Where(e => (!fromDate.HasValue || e.Date >= fromDate)
                                 && (!toDate.HasValue || e.Date <= toDate))
                        .Select(e => e.Comment)
                        .ToList()
                })
                .ToList();

            return View(summary);
        }
    }
}
