using Project1.Areas.Pagination.DTO;

namespace Project1.Areas.Pagination.Service
{
    public class PageService
    {
        public PageDTO Pagination(Int32 Pag, Int32 PageSize, Int32? TotRecCnt , String CatCls, String? Section, String? SearchString)
        {
            PageDTO page = new();
            page.Pag = Pag;
            page.PageSize = PageSize;
            page.TotRecCnt = TotRecCnt;
            page.CatCls = CatCls;
            page.Section = Section;
            page.SearchStr = SearchString;

            page.TotPage = (page.TotRecCnt % page.PageSize) == 0 ? (page.TotRecCnt / page.PageSize) : (page.TotRecCnt / page.PageSize) + 1;
            page.BlockSize = 3;
            page.CurBlock = (page.Pag - 1) / page.BlockSize;
            page.LastBlock = (page.TotPage - 1) / page.BlockSize;

            return page;
        }
    }
}
