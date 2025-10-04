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
    public class CommentLengthTests
    {
        private DbContextOptions<ApplicationDbContext> GetOptions() =>
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        private async Task SeedAsync(ApplicationDbContext db)
        {
            db.Courses.Add(new Course { Title = "MaxLength-kurs" });
            db.Students.Add(new Student { Name = "Testare", Email = "test@example.com" });
            await db.SaveChangesAsync();
        }


        [Fact]

        public async Task Create_Should_Fail_When_Comment_Exceeds_1000()
        {
            var options = GetOptions();
            using var db = new ApplicationDbContext(options);
            await SeedAsync(db);
            var courseId = await db.Courses.Select(c => c.Id).FirstAsync();

            var sut = new EvaluationService(db);
            var vm = new EvaluationFormViewModel
            {
                CourseId = courseId,
                Rating = 3, // >2 så kommentaren är frivillig
                Comment = new string('x', 1001) // 1001 -> borde EJ tillåtas
            };

            var (ok, err) = await sut.CreateEvaluationAsync(vm);

            Assert.False(ok);
            Assert.Contains("exceeds 1000", err ?? "", StringComparison.OrdinalIgnoreCase);

        }

        [Fact]
        public async Task Create_Should_Succeed_When_Comment_Is_1000_Or_Less()
        {
            var options = GetOptions();
            using var db = new ApplicationDbContext(options);
            await SeedAsync(db);
            var courseId = await db.Courses.Select(c => c.Id).FirstAsync();

            var sut = new EvaluationService(db);
            var vm = new EvaluationFormViewModel
            {
                CourseId = courseId,
                Rating = 3,
                Comment = new string('x', 1000) // exakt 1000 -> OK
            };

            var (ok, err) = await sut.CreateEvaluationAsync(vm);

            Assert.True(ok);
            Assert.Null(err);
        }
    }
}
