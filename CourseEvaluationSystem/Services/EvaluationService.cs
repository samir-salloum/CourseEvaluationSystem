using System;
using System.Linq;
using System.Threading.Tasks;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models;
using CourseEvaluationSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CourseEvaluationSystem.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly ApplicationDbContext _db;
        public EvaluationService(ApplicationDbContext db) => _db = db;

        public async Task<(bool Success, string? Error)> CreateEvaluationAsync(EvaluationFormViewModel vm)
        {
            // Basvalidering
            if (vm == null) return (false, "No data provided.");
            if (vm.Rating < 1 || vm.Rating > 5) return (false, "Rating must be between 1 and 5.");

            // Kommentar krävs vid lågt betyg (≤ 2)
            if (vm.Rating <= 2 && string.IsNullOrWhiteSpace(vm.Comment))
                return (false, "Comment is required when rating is 2 or less.");

            // Normalisera kommentaren
            vm.Comment = vm.Comment?.Trim();

            // NY REGEL (TDD): Maxlängd 1000 tecken
            if (!string.IsNullOrWhiteSpace(vm.Comment) && vm.Comment.Length > 1000)
            {
                return (false, "Comment exceeds 1000 characters.");
            }

            // Kurs måste finnas
            var courseExists = await _db.Courses.AnyAsync(c => c.Id == vm.CourseId);
            if (!courseExists) return (false, "Selected course does not exist.");

            // Fallback-student om ingen skickas
            var studentId = vm.StudentId ?? await _db.Students.Select(s => s.Id).FirstOrDefaultAsync();
            if (studentId == 0) return (false, "No students available in database.");

            // Nekar dubblett för samma kurs/student
            var alreadyExists = await _db.Evaluations
                .AnyAsync(e => e.CourseId == vm.CourseId && e.StudentId == studentId);
            if (alreadyExists)
                return (false, "You have already submitted an evaluation for this course.");

            // Skapa och spara
            var eval = new Evaluation
            {
                CourseId = vm.CourseId,
                StudentId = studentId,
                Rating = vm.Rating,
                Comment = vm.Comment,          // redan trimmad
                CreatedAt = DateTime.UtcNow
            };

            _db.Evaluations.Add(eval);
            await _db.SaveChangesAsync();
            return (true, null);
        }

        public async Task<double> GetAverageRatingAsync(int courseId)
        {
            var q = _db.Evaluations.Where(e => e.CourseId == courseId);
            return await q.AnyAsync() ? await q.AverageAsync(e => e.Rating) : 0d;
        }
    }
}
