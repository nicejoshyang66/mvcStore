using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Repositories
{
    public interface IPoinRepository
    {
        Task<List<Poin>> ListAsync(DateTime datefm, DateTime dateto, string? factno);
    }
}