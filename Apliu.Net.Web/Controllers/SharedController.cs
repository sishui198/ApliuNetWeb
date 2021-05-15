using Microsoft.AspNetCore.Mvc;

namespace Apliu.Net.Web.Controllers
{
    public class SharedController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Templet()
        {
            return View();
        }
    }
}
