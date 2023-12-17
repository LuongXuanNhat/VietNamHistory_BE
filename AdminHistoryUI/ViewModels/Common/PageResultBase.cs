namespace AdminHistoryUI.ViewModels.Common
{
    public class PageResultBase
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        // Tổng danh sách
        public int TotalRecords { get; set; }

        public int PageCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
    }
}
