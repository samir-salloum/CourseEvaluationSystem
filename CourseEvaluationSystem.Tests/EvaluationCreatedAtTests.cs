using System;
using System.Threading.Tasks;
using CourseEvaluationSystem.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CourseEvaluationSystem.Tests;

public class EvaluationCreatedAtTests
{
    [Fact]
    public async Task CreatedAt_IsSet_WhenSavingEvaluation()
    {
        using var db = TestHelpers.CreateDb();

        var e = new Evaluation
        {
            CourseId = 1,
            StudentId = 1,
            Rating = 3,
            Comment = "Automatiskt datum"
            // CreatedAt sätts per default i modellen (DateTime.UtcNow)
        };

        db.Evaluations.Add(e);
        await db.SaveChangesAsync();

        var saved = await db.Evaluations.FirstAsync();
        Assert.True((DateTime.UtcNow - saved.CreatedAt).TotalSeconds < 30);
    }
}
