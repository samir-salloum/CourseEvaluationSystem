using System;
using System.ComponentModel.DataAnnotations;

namespace CourseEvaluationSystem.Models
{
    public class Evaluation
    {
        public int Id { get; set; }

        [Display(Name = "Betyg")]
        [Range(1, 5, ErrorMessage = "Betyget måste vara mellan 1 och 5.")]
        public int Rating { get; set; }

        [Display(Name = "Kommentar")]
        [Required(ErrorMessage = "Kommentar är obligatoriskt.")]
        public string Comment { get; set; }

        [Display(Name = "Datum")]
        public DateTime Date { get; set; } = DateTime.Now;

        // Foreign keys
        [Display(Name = "Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Display(Name = "Kurs")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
