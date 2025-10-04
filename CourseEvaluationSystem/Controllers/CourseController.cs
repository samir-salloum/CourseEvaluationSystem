using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseEvaluationSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CourseController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses
                .AsNoTracking()
                .OrderBy(c => c.Title)
                .ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Evaluations)
                .ThenInclude(e => e.Student)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) return NotFound();

            var vm = new CourseDetailsViewModel
            {
                CourseId = course.Id,
                Title = course.Title,
                AverageRating = course.Evaluations.Any()
                    ? course.Evaluations.Average(e => e.Rating)
                    : 0,
                Evaluations = course.Evaluations
                    .OrderByDescending(e => e.CreatedAt)
                    .Select(e => new CourseEvaluationItemVm
                    {
                        Rating = e.Rating,
                        Comment = e.Comment,
                        CreatedAt = e.CreatedAt
                    })
                    .ToList()
            };

            return View(vm); // Views/Course/Details.cshtml
        }
    }
}
