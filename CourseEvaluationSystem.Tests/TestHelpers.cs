using System;
using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseEvaluationSystem.Tests;

public static class TestHelpers
{
    public static ApplicationDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var db = new ApplicationDbContext(options);

        db.Courses.AddRange(
            new Course { Id = 1, Title = "C# Basics" },
            new Course { Id = 2, Title = "ASP.NET MVC" }
        );

        db.Students.AddRange(
            new Student { Id = 1, Name = "Alice Andersson", Email = "alice@example.com" },
            new Student { Id = 2, Name = "Bob Berg", Email = "bob@example.com" }
        );

        db.SaveChanges();
        return db;
    }
}
