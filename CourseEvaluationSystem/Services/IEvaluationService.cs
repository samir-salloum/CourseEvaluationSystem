using System.Threading.Tasks;
using CourseEvaluationSystem.Models.ViewModels;

namespace CourseEvaluationSystem.Services
{
    public interface IEvaluationService
    {
        Task<(bool Success, string? Error)> CreateEvaluationAsync(EvaluationFormViewModel vm);
        Task<double> GetAverageRatingAsync(int courseId);
    }
}
