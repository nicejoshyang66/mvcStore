using Dapper;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public class ZDemoEmployeeExperienceRepo : IZDemoEmployeeExperienceRepo
    {
        private readonly string _connStr;

        public ZDemoEmployeeExperienceRepo(IConfiguration config)
        {
            _connStr = config.GetConnectionString("mvcstore")!;
        }

        public List<z_demo_employee_experience> GetByEmployeeNo(string employeeNo)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    employee_no,
                    name_company,
                    title_company,
                    base_salary,
                    date_start,
                    date_end,
                    remark
                FROM z_demo_employee_experience
                WHERE employee_no = @employeeNo
                ORDER BY date_start, rowid
            ";

            return conn.Query<z_demo_employee_experience>(sql, new { employeeNo }).ToList();
        }

        public z_demo_employee_experience? GetById(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                SELECT
                    rowid,
                    employee_no,
                    name_company,
                    title_company,
                    base_salary,
                    date_start,
                    date_end,
                    remark
                FROM z_demo_employee_experience
                WHERE rowid = @rowid
            ";

            return conn.QueryFirstOrDefault<z_demo_employee_experience>(sql, new { rowid });
        }

        public void Create(z_demo_employee_experience data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                INSERT INTO z_demo_employee_experience
                (
                    employee_no,
                    name_company,
                    title_company,
                    base_salary,
                    date_start,
                    date_end,
                    remark
                )
                VALUES
                (
                    @employee_no,
                    @name_company,
                    @title_company,
                    @base_salary,
                    @date_start,
                    @date_end,
                    @remark
                )
            ";

            conn.Execute(sql, data);
        }

        public void Update(z_demo_employee_experience data)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                UPDATE z_demo_employee_experience
                SET
                    employee_no = @employee_no,
                    name_company = @name_company,
                    title_company = @title_company,
                    base_salary = @base_salary,
                    date_start = @date_start,
                    date_end = @date_end,
                    remark = @remark
                WHERE rowid = @rowid
            ";

            conn.Execute(sql, data);
        }

        public void Delete(int rowid)
        {
            using var conn = new SqlConnection(_connStr);

            string sql = @"
                DELETE FROM z_demo_employee_experience
                WHERE rowid = @rowid
            ";

            conn.Execute(sql, new { rowid });
        }
    }
}