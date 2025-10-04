using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseEvaluationSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        // ENDA navigationen från Student -> Evaluations
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }
}
