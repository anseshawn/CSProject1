namespace Project1.Areas.Pagination.DTO
{
    public class PageDTO
    {
        public Int32 Pag { get; set; } = 1;
        public Int32 PageSize { get; set; } = 5;
        public Int32? TotRecCnt { get; set; }
        public Int32? TotPage { get; set; }
        public Int32 BlockSize { get; set; } = 3;
        public Int32? CurBlock { get; set; }
        public Int32? LastBlock { get; set; }
        public String? CatCls { get; set; } // 게시판종류
        public String? Section { get; set; } // 소분류: part
        public String? SearchStr { get; set; } // 검색어
    }
}
