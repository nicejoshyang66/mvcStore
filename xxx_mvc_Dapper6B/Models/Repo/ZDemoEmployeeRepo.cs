
using Dapper;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.ViewModels;

namespace mvc_Dapper.Models.Repository
{
    public class ZDemoEmployeeRepo : IZDemoEmployeeRepo
    {
        private readonly string _connStr;

        public ZDemoEmployeeRepo(IConfiguration config)
        {
            _connStr = config.GetConnectionString("mvcstore")!;
        }

        public void Create(z_demo_employee data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
              INSERT INTO z_demo_employee
                (
                    is_enabled,
                    mno,
                    mname,
                    no_serial,
                    dept_no,
                    title_no,
                    date_birth,
                    code_gender,
                    salary_base,
                    tel_home,
                    adr_home,
                    remark
                )
                VALUES
                (
                    @is_enabled,
                    @mno,
                    @mname,
                    @no_serial,
                    @dept_no,
                    @title_no,
                    @date_birth,
                    @code_gender,
                    @salary_base,
                    @tel_home,
                    @adr_home,
                    @remark
                )
             ";

            conn.Execute(sql, data);

        }

        public void Delete(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            conn.Open();

            using var tran = conn.BeginTransaction();

            try
            {
                //1. 先用 rowid 查出員工編號 mno
                string sqlGetMno = @"
                SELECT mno
                FROM z_demo_employee
                WHERE rowid = @rowid                
                ";

                string? mno = conn.QueryFirstOrDefault<string>(
                    sqlGetMno, new { rowid }, tran
                );

                // 如果找不到資料，就不繼續刪除
                if (string.IsNullOrWhiteSpace(mno))
                {
                    tran.Rollback();
                    return;
                }

                //2. 用 C# 查到的 mno 當參數傳進 DELETE

                string sqlDelete = @"
                DELETE FROM z_demo_employee_experience
                WHERE employee_no = @mno

                DELETE FROM z_demo_employee_school
                WHERE employee_no = @mno

                DELETE FROM z_demo_employee
                WHERE rowid = @rowid
                ";

                conn.Execute(
                    sqlDelete, new
                    {
                        rowid = rowid,
                        mno = mno
                    }, tran
                );

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }

        }

        public List<z_demo_employee> GetAll()
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    is_enabled,
                    mno,
                    mname,
                    no_serial,
                    dept_no,
                    title_no,
                    date_birth,
                    code_gender,
                    salary_base,
                    tel_home,
                    adr_home,
                    remark
                FROM z_demo_employee
                ORDER BY mno            
            ";

            return conn.Query<z_demo_employee>(sql).ToList();

        }

        public z_demo_employee? GetById(int rowid)
        {
            using var conn = new SqlConnection(_connStr);
            string sql = @"
                SELECT
                    rowid,
                    is_enabled,
                    mno,
                    mname,
                    no_serial,
                    dept_no,
                    title_no,
                    date_birth,
                    code_gender,
                    salary_base,
                    tel_home,
                    adr_home,
                    remark
                FROM z_demo_employee
                WHERE rowid = @rowid
            ";

            return conn.QueryFirstOrDefault<z_demo_employee>(sql, new { rowid });
        }

        public List<SelectOptionVM> GetDepartmentOptions()
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
            SELECT
                mno AS Value,
                mname AS Text
            FROM z_demo_department
            ORDER BY mno            
            ";

            var data = conn.Query<SelectOptionVM>(sql).ToList();

            return data;

        }

        public List<SelectOptionVM> GetTitleOptions()
        {

            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    mno AS Value,
                    mname AS Text
                FROM z_demo_title
                ORDER BY mno
                ";

            var data = conn.Query<SelectOptionVM>(sql).ToList();

            return data;
        }

        public void Update(z_demo_employee data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                            UPDATE z_demo_employee
                SET
                    is_enabled = @is_enabled,
                    mno = @mno,
                    mname = @mname,
                    no_serial = @no_serial,
                    dept_no = @dept_no,
                    title_no = @title_no,
                    date_birth = @date_birth,
                    code_gender = @code_gender,
                    salary_base = @salary_base,
                    tel_home = @tel_home,
                    adr_home = @adr_home,
                    remark = @remark
                WHERE rowid = @rowid                                            
            ";
            conn.Execute(sql,data);

        }
    }
}