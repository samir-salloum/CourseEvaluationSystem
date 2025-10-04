using System.ComponentModel.DataAnnotations;

namespace CourseEvaluationSystem.Models.ViewModels
{
    public class EvaluationFormViewModel
    {
        // Bhöver för att POST:a till rätt kurs
        [Required]
        public int CourseId { get; set; }

        // Visas i Create-vyns rubrik (fylls i GET-action)
        public string CourseTitle { get; set; } = string.Empty;

        // Valfri – används av servicen; om null väljer servicen en default-student
        public int? StudentId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Comment maximum length is 1000 characters.")]
        public string? Comment { get; set; }
    }
}
