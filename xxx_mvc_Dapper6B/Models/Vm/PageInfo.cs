
namespace mvc_Dapper.Models.ViewModel
{
    public class PageInfo
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int TotalCount { get; set; }

        public string ActionName { get; set; } = "List";

        public int TotalPages
        {
            get
            {
                if (PageSize <= 0)
                {
                    return 0;
                }

                return (int)Math.Ceiling((double)TotalCount / PageSize);

            }
        }

        public int StartItem
        {
            get
            {
                if (TotalCount == 0)
                {
                    return 0;
                }

                return ((Page - 1) * PageSize) + 1;
            }
        }

        public int EndItem
        {
            get
            {
                int EndItem = Page * PageSize;

                if (EndItem > TotalCount)
                {
                    EndItem = TotalCount;
                }

                return EndItem;

            }
        }

    }
}