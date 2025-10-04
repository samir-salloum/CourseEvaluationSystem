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
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // 1) Seeda COURSES om tomt
            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    new Course { Title = "Introduction to Programming" },
                    new Course { Title = "Web Development with ASP.NET" }
                );
                context.SaveChanges();
            }

            // 2) Seeda STUDENTS om tomt
            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student { Name = "Alice Andersson", Email = "alice@example.com" },
                    new Student { Name = "Bob Berg", Email = "bob@example.com" }
                );
                context.SaveChanges();
            }

            // 3) Seeda EVALUATIONS om tomt (nu när Courses + Students finns)
            if (!context.Evaluations.Any())
            {
                // Hämta IDs vi behöver
                var course1Id = context.Courses
                    .Where(c => c.Title == "Introduction to Programming")
                    .Select(c => c.Id).FirstOrDefault();

                var course2Id = context.Courses
                    .Where(c => c.Title == "Web Development with ASP.NET")
                    .Select(c => c.Id).FirstOrDefault();

                var student1Id = context.Students
                    .OrderBy(s => s.Id).Select(s => s.Id).FirstOrDefault();

                var student2Id = context.Students
                    .OrderBy(s => s.Id).Skip(1).Select(s => s.Id).FirstOrDefault();

                // Säkerhetskoll
                if (course1Id == 0 || course2Id == 0 || student1Id == 0 || student2Id == 0)
                {
                    // Något saknas – avbryt utan att skapa evaluations
                    return;
                }

                var today = DateTime.UtcNow.Date;

                var samples = new[]
                {
                    new Evaluation
                    {
                        CourseId = course1Id,
                        StudentId = student1Id,
                        Rating = 4,
                        Comment = "Bra introduktion!",
                        CreatedAt = today.AddDays(-2)
                    },
                    new Evaluation
                    {
                        CourseId = course1Id,
                        StudentId = student2Id,
                        Rating = 5,
                        Comment = "Tydliga exempel.",
                        CreatedAt = today.AddDays(-1)
                    },
                    new Evaluation
                    {
                        CourseId = course2Id,
                        StudentId = student1Id,
                        Rating = 3,
                        Comment = "Svårt i början men blev bättre.",
                        CreatedAt = today.AddDays(-1)
                    },
                    new Evaluation
                    {
                        CourseId = course2Id,
                        StudentId = student2Id,
                        Rating = 5,
                        Comment = "Mycket lärorik kurs.",
                        CreatedAt = today
                    }
                };

                context.Evaluations.AddRange(samples);
                context.SaveChanges();
            }
        }
    }
}
