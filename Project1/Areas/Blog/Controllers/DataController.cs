using Microsoft.AspNetCore.Mvc;
using Project1.Areas.Blog.Services;
using Project1.Areas.Community.DTO;
using Project1.Areas.Community.Services;
using Project1.DTO;

namespace Project1.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("[area]/[controller]/[action]")]
    public class DataController : ControllerBase
    {
        String? u_id = String.Empty;
        private readonly Services.DataService dataService = new();

        [HttpGet]
        public async Task<ResponseDTO<List<BoardDTO>>> GetBlogList(String CatCls, String? Section, String? SearchStr, Int32 skip = 0, Int32 take = 5)
        {
            u_id = HttpContext.Session.GetString("key_u_id") ?? "";
            List<BoardDTO> result = await dataService.GetBlogList(CatCls, skip, take, Section, SearchStr, u_id);
            return new ResponseDTO<List<BoardDTO>> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> SetBoardContent([FromForm] String? CatCls, [FromForm] String? B_title, [FromForm] String? B_content, [FromForm] String? u_id)
        {
            Int32 result = await dataService.SetBoardContent(CatCls, B_title, B_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpGet]
        public async Task<ResponseDTO<BoardDTO>> GetBoardContent(Int32 idx)
        {
            u_id = HttpContext.Session.GetString("key_u_id") ?? "";
            BoardDTO result = await dataService.GetBoardContent(idx, u_id);
            return new ResponseDTO<BoardDTO> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> DeleteBoardContent([FromForm] String? CatCls, [FromForm] Int32 idx, [FromForm] String u_id)
        {
            Int32 result = await dataService.DeleteBoardContent(CatCls, idx, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> UpdateBoardContent([FromForm] String? CatCls, [FromForm] Int32 idx, [FromForm] String? B_title, [FromForm] String? B_content, [FromForm] String u_id)
        {
            Int32 result = await dataService.UpdateBoardContent(CatCls, idx, B_title, B_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpGet]
        public async Task<ResponseDTO<List<CommentDTO>>> GetCommentList(String CatCls, Int32? BoardIdx)
        {
            List<CommentDTO> result = await dataService.GetCommentList(CatCls, BoardIdx);
            return new ResponseDTO<List<CommentDTO>> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> SetParentComment([FromForm] String? BoardCatCls, [FromForm] Int32? BoardIdx, [FromForm] String? C_content, [FromForm] String? u_id)
        {
            Int32 result = await dataService.SetParentComment(BoardCatCls, BoardIdx, C_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> UpdateParentComment([FromForm] Int32? idx, [FromForm] String? BoardCatCls, [FromForm] Int32? BoardIdx, [FromForm] String? C_content, [FromForm] String? u_id)
        {
            Int32 result = await dataService.UpdateParentComment(idx, BoardCatCls, BoardIdx, C_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> DeleteParentComment([FromForm] Int32? idx, [FromForm] String? BoardCatCls, [FromForm] Int32? BoardIdx, [FromForm] String? u_id)
        {
            Int32 result = await dataService.DeleteParentComment(idx, BoardCatCls, BoardIdx, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

        [HttpPost]
        public async Task<ResponseDTO<Int32>> SetChildComment([FromForm] Int32? parentIdx, [FromForm] String? BoardCatCls, [FromForm] Int32? BoardIdx, [FromForm] String? C_content, [FromForm] String? u_id)
        {
            Int32 result = await dataService.SetChildComment(parentIdx, BoardCatCls, BoardIdx, C_content, u_id);
            return new ResponseDTO<Int32> { Code = 200, Message = "OK", Data = result };
        }

    }
}
