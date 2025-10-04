using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseEvaluationSystem.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        // ENDA navigationen från Course -> Evaluations
        public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    }
}
