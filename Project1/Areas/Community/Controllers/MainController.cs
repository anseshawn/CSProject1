using Microsoft.AspNetCore.Mvc;
using Project1.Areas.Community.Services;

namespace Project1.Areas.Community.Controllers
{
    [Area("Community")]
    [Route("[area]/[controller]/[action]")]
    public class MainController : Controller
    {
        String? u_id = String.Empty;
        private readonly MainService mainService = new();

        [HttpGet]
        public ActionResult Index()
        {
            u_id = HttpContext.Session.GetString("key_u_id");
            if (u_id == null) return RedirectToAction("RequiredLogin", "Login", new { area = "Identity" });
            ViewData["u_id"] = u_id;
            return View();
        }

    }
}
