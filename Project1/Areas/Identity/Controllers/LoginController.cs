using Microsoft.AspNetCore.Mvc;

namespace Project1.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("[area]/[controller]")]
    public class LoginController : Controller
    {

        [HttpGet("RequiredLogin")]
        public ActionResult RequiredLogin()
        {
            return View("RequiredLogin");
        }

    }
}
