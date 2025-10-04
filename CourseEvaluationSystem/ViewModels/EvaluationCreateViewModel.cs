using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseEvaluationSystem.ViewModels
{
    public class EvaluationCreateViewModel
    {
        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        // Om du inte använder inloggad student än, låt StudentId vara valfritt
        public int? StudentId { get; set; }

        // För dropdowns i vyn
        public IEnumerable<SelectListItem>? Courses { get; set; }
        public IEnumerable<SelectListItem>? Students { get; set; }
    }
}
