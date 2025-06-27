using Microsoft.AspNetCore.Mvc;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models;
using CourseEvaluationSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CourseEvaluationSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lista alla kurser
        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }

        // Visa detaljer + utvärderingar för en kurs
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Evaluations)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            var avg = course.Evaluations.Any() ? course.Evaluations.Average(e => e.Rating) : 0;

            var viewModel = new CourseDetailsViewModel
            {
                CourseId = course.Id,
                Title = course.Title,
                AverageRating = avg,
                Evaluations = course.Evaluations.ToList()
            };

            return View(viewModel);
        }
    }
}
