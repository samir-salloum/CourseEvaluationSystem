using Microsoft.AspNetCore.Mvc;

namespace CourseEvaluationSystem.Controllers
{
    public class AccessDeniedController : Controller
    {
        [Route("AccessDenied")]
        public IActionResult Index()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }
    }
}
