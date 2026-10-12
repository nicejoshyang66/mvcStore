using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.ViewModel
{
    public class PoinListVM
    {
        public DateTime datefm { get; set; }
        public DateTime dateto { get; set; }

        public string? factno { get; set; }

        public List<Poin> Items { get; set; } = new();

        public int TotalCount { get; set; }

        public decimal TotalAmount { get; set; }
    }
}