using CourseEvaluationSystem.Models;
using System.Collections.Generic;

namespace CourseEvaluationSystem.Models.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public double AverageRating { get; set; }
        public List<Evaluation> Evaluations { get; set; }
    }
}
