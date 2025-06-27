using Microsoft.AspNetCore.Mvc;

namespace CourseEvaluationSystem.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {
            return View("Error404");
        }
    }
}
