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
            return View();
        }
    }
}
