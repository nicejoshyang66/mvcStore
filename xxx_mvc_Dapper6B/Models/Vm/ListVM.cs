

using DocumentFormat.OpenXml.Office2016.Excel;

namespace mvc_Dapper.Models.ViewModel
{
    public class ListVM<T>
    {
        public List<T> Items { get; set; } = new List<T>();

        public PageInfo PageInfo { get; set; } = new PageInfo();

        public int PageSize { get; set; } = 10;

    }

}