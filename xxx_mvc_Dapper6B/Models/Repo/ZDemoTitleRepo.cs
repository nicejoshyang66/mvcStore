using Dapper;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public class ZDemoTitleRepo : IZDemoTitleRepo
    {
        private readonly string _connStr;

        public ZDemoTitleRepo(IConfiguration config)
        {
            _connStr = config.GetConnectionString("mvcstore")!;
        }

        public List<z_demo_title> GetAll()
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    mno,
                    mname,
                    remark
                FROM z_demo_title
                ORDER BY mno
            ";

            var data = conn.Query<z_demo_title>(sql).ToList();

            return data;
        }

        public z_demo_title? GetById(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    mno,
                    mname,
                    remark
                FROM z_demo_title
                WHERE rowid = @rowid
            ";

            var data = conn.QueryFirstOrDefault<z_demo_title>(sql, new { rowid });

            return data;
        }

        public void Create(z_demo_title data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                INSERT INTO z_demo_title
                (
                    mno,
                    mname,
                    remark
                )
                VALUES
                (
                    @mno,
                    @mname,
                    @remark
                )
            ";

            conn.Execute(sql, data);
        }

        public void Update(z_demo_title data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                UPDATE z_demo_title
                SET
                    mno = @mno,
                    mname = @mname,
                    remark = @remark
                WHERE rowid = @rowid
            ";

            conn.Execute(sql, data);
        }

        public void Delete(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                DELETE FROM z_demo_title
                WHERE rowid = @rowid
            ";

            conn.Execute(sql, new { rowid });
        }
    }
}