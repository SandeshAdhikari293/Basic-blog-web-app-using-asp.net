namespace Blog.Models
{
    public class Pager
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public Pager()
        {
            
        }

        public Pager(int totalItems, int page, int pageSize = 1)
        {
            int totalPages = (int)Math.Ceiling( (decimal) totalItems / pageSize);
            int currentPage = page;

            int startPage = currentPage - 2;
            int endPage = currentPage + 2;

            if(startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if(endPage > totalPages)
            {
                endPage = totalPages;
                if(endPage > 2)
                {
                    startPage = endPage - 2;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

    }
}
