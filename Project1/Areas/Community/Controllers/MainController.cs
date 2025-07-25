using Microsoft.AspNetCore.Mvc;
using Project1.Areas.Community.DTO;
using Project1.Areas.Community.Services;
using Project1.Areas.Page.DTO;
using Project1.Areas.Page.Service;
using Project1.DTO;

namespace Project1.Areas.Community.Controllers
{
    [Area("Community")]
    [Route("[area]/[controller]/[action]")]
    public class MainController : Controller
    {
        String? u_id = String.Empty;
        private readonly MainService mainService = new();
        private readonly PageService pageService = new();

        [HttpGet]
        public ActionResult Index()
        {
            u_id = HttpContext.Session.GetString("key_u_id");
            if (u_id == null) return RedirectToAction("RequiredLogin", "Login", new { area = "Identity" });
            ViewData["u_id"] = u_id;
            return View();
        }

        [HttpGet]
        public async Task<ResponseDTO<BoardListDTO>> BoardList(String CatCls, Int32? Pag, String? Section, String? SearchStr)
        {
            Int32 TotRecCnt = await mainService.GetBoardListCount(CatCls, Section, SearchStr);
            Int32 PageSize = 10;
            PageDTO page = pageService.Pagination(Pag ?? 1, PageSize, TotRecCnt, CatCls, Section, SearchStr);
            List<BoardDTO> board = await mainService.GetBoardList(CatCls, (Pag ?? 1)-1, PageSize, Section, SearchStr);
            BoardListDTO result = new BoardListDTO
            {
                BoardList = board,
                Page = page
            };
            return new ResponseDTO<BoardListDTO> { Code = 200, Message = "OK", Data = result };
        }
    }
}
