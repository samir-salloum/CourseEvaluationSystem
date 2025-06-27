using System.Collections.Generic;

namespace CourseEvaluationSystem.Models.ViewModels
{
    public class CourseSummaryViewModel
    {
        public string Course { get; set; }
        public double AverageRating { get; set; }
        public List<string> Comments { get; set; }
    }
}
