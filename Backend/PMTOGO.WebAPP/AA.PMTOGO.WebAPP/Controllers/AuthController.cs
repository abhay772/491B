using Microsoft.AspNetCore.Mvc;

namespace AA.PMTOGO.WebAPP.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
