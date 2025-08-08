using Microsoft.AspNetCore.Mvc;

namespace Project1.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("[area]/[controller]/[action]")]
    public class HomeController : Controller
    {
        String? u_id = String.Empty;

        public ActionResult Main()
        {
            u_id = HttpContext.Session.GetString("key_u_id");
            if (u_id == null) return RedirectToAction("RequiredLogin", "Login", new { area = "Identity" });
            ViewData["u_id"] = u_id;
            return View();
        }

        public ActionResult VisitBlog([FromQuery] String u_name)
        {
            ViewData["u_name"] = u_name;
            return View();
        }

        public ActionResult WritePost()
        {
            u_id = HttpContext.Session.GetString("key_u_id");
            if (u_id == null) return RedirectToAction("RequiredLogin", "Login", new { area = "Identity" });
            ViewData["u_id"] = u_id;
            return View();
        }

        public ActionResult BlogContent()
        {
            u_id = HttpContext.Session.GetString("key_u_id");
            if (u_id == null) return RedirectToAction("RequiredLogin", "Login", new { area = "Identity" });
            ViewData["u_id"] = u_id;
            return View();
        }
    }
}
