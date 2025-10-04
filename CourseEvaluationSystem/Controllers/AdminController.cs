using System;
using System.Linq;
using System.Threading.Tasks;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        // GET: /Admin/Index?courseId=&fromDate=&toDate=
        public async Task<IActionResult> Index(int? courseId, DateTime? fromDate, DateTime? toDate)
        {
            // --- Kurslista till dropdown ---
            var courses = await _context.Courses
                .AsNoTracking()
                .OrderBy(c => c.Title)
                .Select(c => new { c.Id, c.Title })
                .ToListAsync();

            ViewBag.Courses = new SelectList(courses, "Id", "Title");

            // --- Grundquery: Evaluations + Course ---
            var evals = _context.Evaluations
                .AsNoTracking()
                .Include(e => e.Course)
                .AsQueryable();

            // Kursfilter
            if (courseId.HasValue && courseId.Value != 0)
            {
                evals = evals.Where(e => e.CourseId == courseId.Value);
            }

            // Datumfilter (inkludera hela "toDate"-dagen)
            if (fromDate.HasValue)
            {
                var fd = fromDate.Value.Date;
                evals = evals.Where(e => e.CreatedAt >= fd);
            }
            if (toDate.HasValue)
            {
                var td = toDate.Value.Date.AddDays(1).AddTicks(-1);
                evals = evals.Where(e => e.CreatedAt <= td);
            }

            // --- Sammanställ per kurs ---
            var summary = await evals
                .GroupBy(e => new { e.CourseId, e.Course.Title })
                .Select(g => new CourseSummaryViewModel
                {
                    CourseId = g.Key.CourseId,
                    Course = g.Key.Title,
                    AverageRating = g.Any() ? g.Average(x => x.Rating) : 0,
                    Comments = g
                        .OrderByDescending(x => x.CreatedAt)
                        .Select(x => x.Comment)
                        .Where(c => c != null && c != "")
                        .Take(20) // visa max 20 senaste kommentarer
                        .ToList()
                })
                .OrderBy(m => m.Course)
                .ToListAsync();

            // Skicka tillbaka aktiva filter till vyn (för att visa "Aktivt filter")
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(summary);
        }
    }
}
