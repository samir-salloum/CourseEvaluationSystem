using System.Threading.Tasks;
using CourseEvaluationSystem.Controllers;
using CourseEvaluationSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseEvaluationSystem.Tests
{
    public class AdminControllerTests
    {
        private static ApplicationDbContext NewDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("admin_db_" + System.Guid.NewGuid())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Index_ReturnsViewResult()
        {
            using var db = NewDb();
            var ctrl = new AdminController(db);
            var result = await ctrl.Index(null, null, null);
            Assert.IsType<ViewResult>(result);
        }
    }
}
