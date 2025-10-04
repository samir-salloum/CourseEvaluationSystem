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
    public class EvaluationServiceTests
    {
        private DbContextOptions<ApplicationDbContext> GetOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        private async Task SeedCourseAndStudentAsync(ApplicationDbContext ctx)
        {
            ctx.Courses.Add(new Course { Title = "Testkurs" });
            ctx.Students.Add(new Student { Name = "Test Student", Email = "test@student.se" });
            await ctx.SaveChangesAsync();
        }

        [Fact]
        public async Task Create_Succeeds_WhenValid()
        {
            // Arrange
            var options = GetOptions();
            using var ctx = new ApplicationDbContext(options);
            await SeedCourseAndStudentAsync(ctx);

            var courseId = await ctx.Courses.Select(c => c.Id).FirstAsync();
            var svc = new EvaluationService(ctx);

            var vm = new EvaluationFormViewModel
            {
                CourseId = courseId,
                Rating = 4,
                Comment = "Bra kurs!"
                // StudentId = null -> service väljer default student
            };

            // Act
            var (ok, err) = await svc.CreateEvaluationAsync(vm);

            // Assert
            Assert.True(ok);
            Assert.Null(err);
            Assert.Equal(1, ctx.Evaluations.Count());
            var ev = await ctx.Evaluations.FirstAsync();
            Assert.Equal(4, ev.Rating);
            Assert.NotEqual(default, ev.CreatedAt);
        }

        [Fact]
        public async Task Create_Fails_WhenRating_OutOfRange()
        {
            // Arrange
            var options = GetOptions();
            using var ctx = new ApplicationDbContext(options);
            await SeedCourseAndStudentAsync(ctx);

            var courseId = await ctx.Courses.Select(c => c.Id).FirstAsync();
            var svc = new EvaluationService(ctx);

            var vm = new EvaluationFormViewModel
            {
                CourseId = courseId,
                Rating = 0, // ogiltigt (<1)
                Comment = "Fel rating"
            };

            // Act
            var (ok, err) = await svc.CreateEvaluationAsync(vm);

            // Assert
            Assert.False(ok);
            Assert.NotNull(err);
            Assert.Equal(0, ctx.Evaluations.Count());
        }

        [Fact]
        public async Task Average_Works_ForCourse()
        {
            // Arrange
            var options = GetOptions();
            using var ctx = new ApplicationDbContext(options);
            await SeedCourseAndStudentAsync(ctx);

            var courseId = await ctx.Courses.Select(c => c.Id).FirstAsync();
            var studentId = await ctx.Students.Select(s => s.Id).FirstAsync();

            ctx.Evaluations.AddRange(
                new Evaluation { CourseId = courseId, StudentId = studentId, Rating = 3, Comment = "ok", CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Evaluation { CourseId = courseId, StudentId = studentId, Rating = 5, Comment = "bra", CreatedAt = DateTime.UtcNow.AddDays(-1) }
            );
            await ctx.SaveChangesAsync();

            var svc = new EvaluationService(ctx);

            // Act
            var avg = await svc.GetAverageRatingAsync(courseId);

            // Assert
            Assert.Equal(4.0, avg, precision: 5);
        }
    }
}
