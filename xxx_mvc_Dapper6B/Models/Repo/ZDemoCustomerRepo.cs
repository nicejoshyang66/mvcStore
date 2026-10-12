using Dapper;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public class ZDemoCustomerRepo : IZDemoCustomerRepo
    {
        private readonly string _connStr;

        public ZDemoCustomerRepo(IConfiguration config)
        {
            _connStr = config.GetConnectionString("mvcstore")!;
        }

        public List<z_demo_customer> GetAll()
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    is_enabled,
                    mno,
                    msname,
                    mname,
                    no_serial,
                    name_invoice,
                    tel_company,
                    name_charge,
                    name_contact,
                    adr_company,
                    adr_invoice,
                    remark
                FROM z_demo_customer
                ORDER BY mno
            ";

            var data = conn.Query<z_demo_customer>(sql).ToList();

            return data;
        }

        public z_demo_customer? GetById(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    is_enabled,
                    mno,
                    msname,
                    mname,
                    no_serial,
                    name_invoice,
                    tel_company,
                    name_charge,
                    name_contact,
                    adr_company,
                    adr_invoice,
                    remark
                FROM z_demo_customer
                WHERE rowid = @rowid
            ";

            var data = conn.QueryFirstOrDefault<z_demo_customer>(sql, new { rowid });

            return data;
        }

        public void Create(z_demo_customer data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                INSERT INTO z_demo_customer
                (
                    is_enabled,
                    mno,
                    msname,
                    mname,
                    no_serial,
                    name_invoice,
                    tel_company,
                    name_charge,
                    name_contact,
                    adr_company,
                    adr_invoice,
                    remark
                )
                VALUES
                (
                    @is_enabled,
                    @mno,
                    @msname,
                    @mname,
                    @no_serial,
                    @name_invoice,
                    @tel_company,
                    @name_charge,
                    @name_contact,
                    @adr_company,
                    @adr_invoice,
                    @remark
                )
            ";

            conn.Execute(sql, data);
        }

        public void Update(z_demo_customer data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                UPDATE z_demo_customer
                SET
                    is_enabled = @is_enabled,
                    mno = @mno,
                    msname = @msname,
                    mname = @mname,
                    no_serial = @no_serial,
                    name_invoice = @name_invoice,
                    tel_company = @tel_company,
                    name_charge = @name_charge,
                    name_contact = @name_contact,
                    adr_company = @adr_company,
                    adr_invoice = @adr_invoice,
                    remark = @remark
                WHERE rowid = @rowid
            ";

            conn.Execute(sql, data);
        }

        public void Delete(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                DELETE FROM z_demo_customer
                WHERE rowid = @rowid
            ";

            conn.Execute(sql, new { rowid });
        }
    }
}