using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CourseEvaluationSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }
}
