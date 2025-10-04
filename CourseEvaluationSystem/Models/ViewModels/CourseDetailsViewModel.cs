using System.Collections.Generic;

namespace CourseEvaluationSystem.Models.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public double AverageRating { get; set; }
        public List<CourseEvaluationItemVm> Evaluations { get; set; } = new();
    }
}
