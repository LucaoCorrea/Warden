using Microsoft.AspNetCore.Mvc;
using Warden.Filters;

namespace Warden.Controllers
{
    [PageForLoggedInUser]
    public class RestrictedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
