using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using CourseEvaluationSystem.Models;

namespace CourseEvaluationSystem.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Kolla om det redan finns kurser
                if (context.Courses.Any())
                {
                    return;   // Stoppa om det redan finns data
                }

                context.Courses.AddRange(
                    new Course { Title = "Introduction to Programming" },
                    new Course { Title = "Web Development with ASP.NET" }
                );

                context.SaveChanges();
            }
        }
    }
}
