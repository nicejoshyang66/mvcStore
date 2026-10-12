
using Dapper;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public class ZDemoEmployeeSchoolRepo : IZDemoEmployeeSchoolRepo
    {
        private readonly string _connStr;

        public ZDemoEmployeeSchoolRepo(IConfiguration config)
        {
            _connStr = config.GetConnectionString("mvcstore")!;
        }

        public List<z_demo_employee_school> GetByEmployeeNo(string employeeNo)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    employee_no,
                    name_school,
                    name_subject,
                    date_start,
                    date_end,
                    code_status,
                    remark
                FROM z_demo_employee_school
                WHERE employee_no = @employeeNo
                ORDER BY date_start, rowid
            ";

            return conn.Query<z_demo_employee_school>(sql, new { employeeNo }).ToList();
        }

        public z_demo_employee_school? GetById(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    employee_no,
                    name_school,
                    name_subject,
                    date_start,
                    date_end,
                    code_status,
                    remark
                FROM z_demo_employee_school
                WHERE rowid = @rowid
            ";

            return conn.QueryFirstOrDefault<z_demo_employee_school>(sql, new { rowid });
        }

        public void Create(z_demo_employee_school data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                INSERT INTO z_demo_employee_school
                (
                    employee_no,
                    name_school,
                    name_subject,
                    date_start,
                    date_end,
                    code_status,
                    remark
                )
                VALUES
                (
                    @employee_no,
                    @name_school,
                    @name_subject,
                    @date_start,
                    @date_end,
                    @code_status,
                    @remark
                )
            ";

            conn.Execute(sql, data);
        }

        public void Update(z_demo_employee_school data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                UPDATE z_demo_employee_school
                SET
                    employee_no = @employee_no,
                    name_school = @name_school,
                    name_subject = @name_subject,
                    date_start = @date_start,
                    date_end = @date_end,
                    code_status = @code_status,
                    remark = @remark
                WHERE rowid = @rowid
            ";

            conn.Execute(sql, data);
        }

        public void Delete(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                DELETE FROM z_demo_employee_school
                WHERE rowid = @rowid
            ";

            conn.Execute(sql, new { rowid });
        }
    }
}