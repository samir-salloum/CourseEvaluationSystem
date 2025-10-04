using System.Collections.Generic;

namespace CourseEvaluationSystem.Models.ViewModels
{
    public class CourseSummaryViewModel
    {
        public int CourseId { get; set; }                  // 🔹 Ny property
        public string Course { get; set; } = string.Empty; // 🔹 Safe init
        public double AverageRating { get; set; }
        public List<string> Comments { get; set; } = new(); // 🔹 Initierad lista
    }
}
