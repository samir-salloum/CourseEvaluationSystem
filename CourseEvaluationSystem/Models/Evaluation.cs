using System;
using System.ComponentModel.DataAnnotations;

namespace CourseEvaluationSystem.Models
{
    public class Evaluation
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        // Sätts automatiskt vid skapande – funkar även med InMemory provider och när
        // man går förbi servicen i testerna.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK + navigationer
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
    }
}
