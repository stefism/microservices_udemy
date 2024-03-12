using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
