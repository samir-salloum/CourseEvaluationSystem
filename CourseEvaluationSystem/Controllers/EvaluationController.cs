using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Services;
using CourseEvaluationSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CourseEvaluationSystem.Controllers
{
    [Authorize] // kräver inloggning för att skapa utvärdering (rekommenderat)
    public class EvaluationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEvaluationService _evaluationService;
        private readonly UserManager<IdentityUser> _userManager;

        public EvaluationController(
            ApplicationDbContext context,
            IEvaluationService evaluationService,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _evaluationService = evaluationService;
            _userManager = userManager;
        }

        // GET: /Evaluation/Create?courseId=123
        [HttpGet]
        public async Task<IActionResult> Create(int courseId, CancellationToken ct)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == courseId, ct);

            if (course == null) return NotFound();

            var vm = new EvaluationFormViewModel
            {
                CourseId = course.Id,
                CourseTitle = course.Title
            };
            return View(vm);
        }

        // POST: /Evaluation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EvaluationFormViewModel model, CancellationToken ct)
        {
            // 1) Servervalidering
            if (!ModelState.IsValid)
            {
                var course = await _context.Courses.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == model.CourseId, ct);
                model.CourseTitle = course?.Title ?? "";
                return View(model);
            }

            // 2) Hämta inloggad användare -> mappa till Student -> sätt StudentId
            if (User?.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                var email = await _userManager.GetEmailAsync(user);

                // försök hitta befintlig Student med samma e-post
                var student = await _context.Students
                    .FirstOrDefaultAsync(s => s.Email == email, ct);

                // finns ingen -> skapa en Student kopplad till e-posten
                if (student == null)
                {
                    student = new CourseEvaluationSystem.Models.Student
                    {
                        Name = user?.UserName ?? email,
                        Email = email
                    };
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync(ct);
                }

                model.StudentId = student.Id; // VIKTIGT: nu kan dubblett-regeln fungera per användare
            }

            // 3) Anropa tjänsten
            var (ok, err) = await _evaluationService.CreateEvaluationAsync(model);
            if (!ok)
            {
                ModelState.AddModelError(string.Empty, err ?? "Ett fel inträffade.");
                var course = await _context.Courses.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == model.CourseId, ct);
                model.CourseTitle = course?.Title ?? "";
                return View(model);
            }

            // 4) Redirect + tack
            var savedCourse = await _context.Courses.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == model.CourseId, ct);
            TempData["CourseTitle"] = savedCourse?.Title ?? "";
            return RedirectToAction(nameof(Thanks));
        }

        [AllowAnonymous] // tack-sidan kan vara publik
        public IActionResult Thanks()
        {
            ViewBag.CourseTitle = TempData["CourseTitle"] as string ?? "";
            return View();
        }
    }
}
