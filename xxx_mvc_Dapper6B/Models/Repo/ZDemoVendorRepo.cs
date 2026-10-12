
using Dapper;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public class ZDemoVendorRepo:IZDemoVendorRepo
    {
        private readonly string _connStr;

        public ZDemoVendorRepo(IConfiguration config)
        {
            _connStr=config.GetConnectionString("mvcstore")!;
        }

        public void Create(z_demo_vendor data)
        {
            using var conn=new SqlConnection(_connStr);
            string sqlstr=@"
                INSERT INTO z_demo_vendor
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

            conn.Execute(sqlstr,data);

        }

        public void Delete(int rowid)
        {
            using var conn=new SqlConnection(_connStr);

            string sqlstr=@"
                DELETE FROM z_demo_vendor
                WHERE rowid = @rowid            
            ";

            conn.Execute(sqlstr,new {rowid});
        }

        public List<z_demo_vendor> GetAll()
        {
            using var conn=new SqlConnection(_connStr);

            string sqlstr=@"
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
                FROM z_demo_vendor
                ORDER BY mno
            ";

            var data=conn.Query<z_demo_vendor>(sqlstr).ToList();     

            return data;

        }

        public z_demo_vendor? GetById(int rowid)
        {
            using var conn=new SqlConnection(_connStr);

            string sqlstr=@"
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
                FROM z_demo_vendor
                WHERE rowid = @rowid
            ";

            var data=conn.QueryFirstOrDefault<z_demo_vendor>(sqlstr,new{rowid});

            return data;

        }

        public void Update(z_demo_vendor data)
        {
            using var conn=new SqlConnection(_connStr);

            string sqlstr=@"
                UPDATE z_demo_vendor
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

            conn.Execute(sqlstr,data);

        }
    }
}