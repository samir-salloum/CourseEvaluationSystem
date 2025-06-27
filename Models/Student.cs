using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CourseEvaluationSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }
}
