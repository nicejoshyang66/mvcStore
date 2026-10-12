using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Repositories
{
    public class PoinRepository : IPoinRepository
    {
        private readonly IConfiguration _configuration;

        public PoinRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("mvcstore"));
        }

        public async Task<List<Poin>> ListAsync(DateTime datefm, DateTime dateto, string? factno)
        {
            var sql = @"
            select 
                S.TRAN_NO     as tran_no,
                S.PO_DATE     as po_date,
                S.FACT_NO     as fact_no,
                M.BRIEF_NAME  as brief_name,
                S.TRAN_AMT    as tran_amt,
                S.FACT_MEMO   as fact_memo
            from POIN S
            left join FACTORY M on S.FACT_NO = M.FACT_NO
            where S.PO_DATE >= @datefm
            and S.PO_DATE <= @dateto
            and (@factno is null or @factno = '' or S.FACT_NO = @factno)
            order by S.PO_DATE, S.TRAN_NO
            ";

            using var conn = CreateConnection();

            var data = await conn.QueryAsync<Poin>(sql, new
            {
                datefm,
                dateto,
                factno
            });

            return data.ToList();
        }
    }
}