using Project1.Areas.Page.DTO;

namespace Project1.Areas.Community.DTO
{
    public class BoardDTO
    {
        public Int32 idx { get; set; }
        public String? CatCls { get; set; }
        public String? B_title { get; set; }
        public String? B_content { get; set; }
        public String? B_author { get; set; }
        public DateTimeOffset? EnterDateTime { get; set; }
        public String? EnterUser { get; set; }
    }

    public class BoardListDTO
    {
        public List<BoardDTO>? BoardList { get; set; }
        public PageDTO? Page { get; set; }
    }
}
