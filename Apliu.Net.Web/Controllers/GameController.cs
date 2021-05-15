using Microsoft.AspNetCore.Mvc;

namespace Apliu.Net.Web.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Pins()
        {
            return View();
        }
    }
}
