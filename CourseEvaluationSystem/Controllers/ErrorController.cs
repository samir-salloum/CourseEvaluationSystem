using Microsoft.AspNetCore.Mvc;

namespace CourseEvaluationSystem.Controllers
{
    public class ErrorController : Controller
    {
        // Fångar t.ex. /Error/404, /Error/403, /Error/500
        [Route("Error/{code:int}")]
        public IActionResult HttpStatusCodeHandler(int code)
        {
            // kan ha olika vyer för olika koder om du vill:
            // if (code == 404) return View("Error404");
            // if (code == 403) return View("Error403");
            // return View("Error"); // generisk

            // Enkel lösning: visa generisk vy och skicka med kod
            ViewBag.StatusCode = code;
            return View("Error"); // Views/Shared/Error.cshtml (standard)
        }
    }
}
