using CourseEvaluationSystem.Data;
using CourseEvaluationSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CourseEvaluationSystem.Tests;

public class AdminFilterTests
{
    [Fact]
    public async Task Filter_By_Date_Range_Works()
    {
        using var db = TestHelpers.CreateDb();

        var oldEval = new Evaluation
        {
            CourseId = 1,
            StudentId = 1,
            Rating = 3,
            Comment = "Old evaluation",
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        };

        var newEval = new Evaluation
        {
            CourseId = 1,
            StudentId = 2,
            Rating = 5,
            Comment = "New evaluation",
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };

        db.Evaluations.AddRange(oldEval, newEval);
        await db.SaveChangesAsync();

        var fromDate = DateTime.UtcNow.AddDays(-5);

        var filtered = await db.Evaluations
            .Where(e => e.CreatedAt >= fromDate)
            .ToListAsync();

        Assert.Single(filtered); // bara newEval ska med
        Assert.Equal("New evaluation", filtered.First().Comment);
    }
}
