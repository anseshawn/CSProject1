using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Project1.Areas.Main.DTO;
using Project1.Areas.Main.Services;
using Project1.DTO;

namespace Project1.Areas.Main.Controllers
{
    [Area("Main")]
    [Route("[area]/[action]")]
    public class MainController : Controller
    {
        String? u_id = String.Empty;
        private readonly MainService mainService = new();

        [HttpGet]
        public ActionResult Index()
        {
            u_id = HttpContext.Session.GetString("key_u_id") ?? String.Empty;
            ViewData["u_id"] = u_id;
            return View();
        }

        [HttpGet]
        public ActionResult LoginView()
        {
            String? savedId = Request.Cookies["u_id"];
            if (!string.IsNullOrEmpty(savedId))
            {
                ViewData["savedId"] = savedId;
            }
            return View();
        }

        [HttpGet]
        public ActionResult JoinView()
        {
            return View();
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> GetMemberInfo(String u_id)
        {
            Int32 result = 0;
            MemberDTO? member = await mainService.GetMemberInfo(u_id);
            if (member != null)
            {
                result = 1;
            }
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> GetMemberInfoByName(String u_name)
        {
            Int32 result = 0;
            MemberDTO? member = await mainService.GetMemberInfoByName(u_name);
            if (member != null)
            {
                result = 1;
            }
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ActionResult> MemberJoin(MemberDTO? memberDTO)
        {
            Int32 res = 0;
            if (memberDTO == null)
            {
                return Json(new
                {
                    success = false,
                    errorCode = "INVALID_REQUEST",
                    message = "올바른 접근이 아닙니다."
                });
            }

            var hash = new PasswordHasher();
            memberDTO.u_pwd = hash.HashPassword(memberDTO.u_pwd);
            res = await mainService.NewMemberJoin(memberDTO);

            if (res != 0)
            {
                return Json(new
                {
                    success = true,
                    message = "회원가입 성공!",
                    redirectUrl = Url.Action("LoginView", "Main")
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errorCode = "INVALID_CREDENTIALS",
                    message = "회원 가입 실패. 관리자에게 문의하세요."
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult> MemberLogin([FromForm] Boolean idSave, [FromForm] MemberDTO? memberDTO)
        {
            if (memberDTO == null)
            {
                return Json(new
                {
                    success = false,
                    errorCode = "INVALID_REQUEST",
                    message = "올바른 접근이 아닙니다."
                });
            }
            if (idSave)
            {
                Response.Cookies.Append("u_id", memberDTO.u_id ?? String.Empty, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true
                    //Secure = false,
                    //SameSite = SameSiteMode.Strict
                });
            }
            else
            {
                 Response.Cookies.Delete("u_id");
            }

            var hash = new PasswordHasher();
            //memberDTO.u_pwd = hash.HashPassword(memberDTO.u_pwd);
            MemberDTO? searchDTO = await mainService.GetMemberInfo(memberDTO.u_id ?? string.Empty);

            var pwdResult = hash.VerifyHashedPassword(searchDTO?.u_pwd, memberDTO.u_pwd);

            if (pwdResult == PasswordVerificationResult.Success)
            {
                HttpContext.Session.SetString("key_u_id", memberDTO.u_id ?? string.Empty);
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "Main")
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errorCode = "INVALID_CREDENTIALS",
                    message = "아이디 또는 비밀번호가 맞지 않습니다."
                });
            }
        }

        [HttpPost]
        public ActionResult MemberLogout([FromForm] String? u_id)
        {
            if (u_id == null)
            {
                return Json(new
                {
                    success = false,
                    errorCode = "INVALID_REQUEST",
                    message = "올바른 접근이 아닙니다."
                });
            }

            HttpContext.Session.Clear();
            return Json(new
            {
                success = true,
                message = "로그아웃 되었습니다.",
                redirectUrl = Url.Action("Index", "Main")
            });
        }
    }
}
