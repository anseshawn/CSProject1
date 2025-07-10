using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project1.Areas.Community.DTO;
using Project1.Areas.Community.Services;
using Project1.Areas.Main.Services;
using Project1.Areas.Page.DTO;
using Project1.Areas.Page.Service;
using Project1.DTO;
using static System.Collections.Specialized.BitVector32;

namespace Project1.Areas.Community.Controllers
{
    [Area("Community")]
    [Route("[area]/[controller]/[action]")]
    public class BoardController : Controller
    {
        String? u_id = String.Empty;
        private readonly BoardService boardService = new();

        [HttpGet]
        public ActionResult WriteBoard()
        {
            u_id = HttpContext.Session.GetString("key_u_id");
            if (u_id == null) return RedirectToAction("RequiredLogin", "Login", new { area = "Identity" });
            ViewData["u_id"] = u_id;
            return View();
        }

        [HttpGet]
        public ActionResult BoardContent([FromQuery] Int32 idx)
        {
            u_id = HttpContext.Session.GetString("key_u_id");
            if (u_id == null) return RedirectToAction("RequiredLogin", "Login", new { area = "Identity" });
            ViewData["u_id"] = u_id;
            return View();
        }

		[HttpGet]
		public ActionResult EditBoard(Int32 idx)
		{
			u_id = HttpContext.Session.GetString("key_u_id");
			if (u_id == null) return RedirectToAction("RequiredLogin", "Login", new { area = "Identity" });
			ViewData["u_id"] = u_id;
			return View();
		}

		[HttpPost]
        public async Task<ResponseDTO<Int32>> SetBoardContent([FromForm] String? CatCls, [FromForm] String? B_title, [FromForm] String? B_content, [FromForm] String? u_id)
        {
            Int32 result = await boardService.SetBoardContent(CatCls, B_title, B_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpGet]
        public async Task<ResponseDTO<BoardDTO>> GetBoardContent(Int32 idx)
        {
            BoardDTO result = await boardService.GetBoardContent(idx);
            return new ResponseDTO<BoardDTO> { Code = 200, Message = "OK", Data = result };
        }
        
        [HttpPost]
        public async Task<ResponseDTO<Int32>> DeleteBoardContent([FromForm] String? CatCls, [FromForm] Int32 idx, [FromForm] String u_id)
        {
            Int32 result = await boardService.DeleteBoardContent(CatCls, idx, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }          

        [HttpPost]
        public async Task<ResponseDTO<Int32>> UpdateBoardContent([FromForm] String? CatCls, [FromForm] Int32 idx, [FromForm] String? B_title, [FromForm] String? B_content, [FromForm] String u_id)
        {
            Int32 result = await boardService.UpdateBoardContent(CatCls, idx, B_title, B_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }        

        [HttpGet]
        public async Task<ResponseDTO<List<CommentDTO>>> GetCommentList(String CatCls, Int32? BoardIdx)
        {
            List<CommentDTO> result = await boardService.GetCommentList(CatCls, BoardIdx);
            return new ResponseDTO<List<CommentDTO>> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> SetParentComment([FromForm] String? BoardCatCls, [FromForm] Int32? BoardIdx, [FromForm] String? C_content, [FromForm] String? u_id)
        {
            Int32 result = await boardService.SetParentComment(BoardCatCls, BoardIdx, C_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> UpdateParentComment([FromForm] Int32? idx, [FromForm] String? BoardCatCls, [FromForm] Int32? BoardIdx, [FromForm] String? C_content, [FromForm] String? u_id)
        {
            Int32 result = await boardService.UpdateParentComment(idx, BoardCatCls, BoardIdx, C_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

    }
}
