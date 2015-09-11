using System.Web.Mvc;

namespace VendingMachineApp.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ViewResult Index(string message)
        {
            ViewData["ErrorMessage"] = message;

            return View("Error");
        }
    }
}