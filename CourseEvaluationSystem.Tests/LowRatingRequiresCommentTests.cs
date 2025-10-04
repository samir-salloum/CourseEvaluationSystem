using System.Threading.Tasks;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models;
using CourseEvaluationSystem.Models.ViewModels;
using CourseEvaluationSystem.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseEvaluationSystem.Tests
{
    public class LowRatingRequiresCommentTests
    {
        private static ApplicationDbContext NewDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("low_rating_rule_" + System.Guid.NewGuid())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Should_ReturnError_When_RatingIsTwoOrLess_And_CommentIsEmpty()
        {
            using var db = NewDb();
            db.Courses.Add(new Course { Title = "X" });
            db.Students.Add(new Student { Name = "S", Email = "s@test.se" });
            await db.SaveChangesAsync();

            var svc = new EvaluationService(db);

            var vm = new EvaluationFormViewModel { CourseId = 1, Rating = 2, Comment = "" };
            var (ok, err) = await svc.CreateEvaluationAsync(vm);

            Assert.False(ok);
            Assert.Equal("Comment is required when rating is 2 or less.", err);
        }
    }
}
