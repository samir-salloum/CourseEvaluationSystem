namespace CourseEvaluationSystem.Models.ViewModels
{
    public class EvaluationFormViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
