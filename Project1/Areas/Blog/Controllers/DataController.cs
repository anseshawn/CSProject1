using Microsoft.AspNetCore.Mvc;
using Project1.Areas.Blog.Services;
using Project1.Areas.Community.DTO;
using Project1.DTO;

namespace Project1.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("[area]/[controller]/[action]")]
    public class DataController : ControllerBase
    {
        String? u_id = String.Empty;
        private readonly DataService dataService = new();

        [HttpGet]
        public async Task<ResponseDTO<List<BoardDTO>>> GetBlogList(String CatCls, String? Section, String? SearchStr, Int32 skip = 0, Int32 take = 10)
        {
            u_id = HttpContext.Session.GetString("key_u_id");
            List<BoardDTO> result = await dataService.GetBlogList(CatCls, skip, take, Section, SearchStr, u_id);
            return new ResponseDTO<List<BoardDTO>> { Code = 200, Message = "OK", Data = result };
        }
    }
}
