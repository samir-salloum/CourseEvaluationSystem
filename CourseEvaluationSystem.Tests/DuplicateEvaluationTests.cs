using System;
using System.Linq;
using System.Threading.Tasks;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models;
using CourseEvaluationSystem.Models.ViewModels;
using CourseEvaluationSystem.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseEvaluationSystem.Tests
{
    public class DuplicateEvaluationTests
    {
        private DbContextOptions<ApplicationDbContext> GetOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        private async Task SeedAsync(ApplicationDbContext ctx)
        {
            ctx.Courses.Add(new Course { Title = "Testkurs" });
            ctx.Students.Add(new Student { Name = "Student", Email = "s@test.se" });
            await ctx.SaveChangesAsync();
        }

        [Fact]
        public async Task Should_ReturnError_When_Submitting_Second_Evaluation_For_Same_Course_And_Student()
        {
            // Arrange
            var options = GetOptions();
            using var ctx = new ApplicationDbContext(options);
            await SeedAsync(ctx);

            var courseId = await ctx.Courses.Select(c => c.Id).FirstAsync();
            var studentId = await ctx.Students.Select(s => s.Id).FirstAsync();

            var svc = new EvaluationService(ctx);

            // 1:a (ska lyckas)
            var first = await svc.CreateEvaluationAsync(new EvaluationFormViewModel
            {
                CourseId = courseId,
                StudentId = studentId,
                Rating = 4,
                Comment = "Första"
            });
            Assert.True(first.Success);

            // 2:a för samma kurs/student (ska nekas)
            var second = await svc.CreateEvaluationAsync(new EvaluationFormViewModel
            {
                CourseId = courseId,
                StudentId = studentId,
                Rating = 5,
                Comment = "Andra"
            });

            Assert.False(second.Success);
            Assert.Contains("duplicate", second.Error ?? "", StringComparison.OrdinalIgnoreCase);

        }
    }
}
