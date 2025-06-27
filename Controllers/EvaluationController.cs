using Microsoft.AspNetCore.Mvc;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models;
using CourseEvaluationSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace CourseEvaluationSystem.Controllers
{
    public class EvaluationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EvaluationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Evaluation/Create?courseId=1
        public async Task<IActionResult> Create(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var viewModel = new EvaluationFormViewModel
            {
                CourseId = course.Id,
                CourseTitle = course.Title
            };

            return View(viewModel);
        }

        // POST: Evaluation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EvaluationFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var evaluation = new Evaluation
                {
                    CourseId = model.CourseId,
                    Rating = model.Rating,
                    Comment = model.Comment,
                    Date = DateTime.Now
                };

                _context.Evaluations.Add(evaluation);
                await _context.SaveChangesAsync();

                var course = await _context.Courses.FindAsync(model.CourseId);
                var courseTitle = course?.Title ?? "okänd kurs";
                return RedirectToAction("Thanks", new { courseTitle });
            }

            // Om validering misslyckas
            var c = await _context.Courses.FindAsync(model.CourseId);
            model.CourseTitle = c?.Title ?? "okänd kurs";
            return View(model);
        }

        // GET: Evaluation/Thanks?courseTitle=xxx
        public IActionResult Thanks(string courseTitle)
        {
            ViewBag.CourseTitle = courseTitle;
            return View();
        }
    }
}
