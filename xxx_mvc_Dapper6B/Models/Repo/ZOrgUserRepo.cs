

using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public class ZOrgUserRepo : IZOrgUserRepo
    {
        private readonly IConfiguration _config;

        public ZOrgUserRepo(IConfiguration config)
        {
            _config = config;
        }

        public bool ChangePassword(int rowid, string newPassword)
        {

            string @sqlstr = @"
            update z_org_user
            set pass_user = @newPassword
            where rowid = @rowid
            ";

            using var conn = Conn();

            int affected = conn.Execute(sqlstr, new
            {
                rowid,
                newPassword
            });

            return affected > 0;

        }

        public bool Create(z_org_user mas)
        {
            string sqlstr = @"
             INSERT INTO z_org_user
                (
                    code_user,
                    is_enabled,
                    mno,
                    mname,
                    pass_user,
                    tel_user,
                    mobile_user,
                    email_user,
                    remark
                )
                VALUES
                (
                    @code_user,
                    @is_enabled,
                    @mno,
                    @mname,
                    @pass_user,
                    @tel_user,
                    @mobile_user,
                    @email_user,
                    @remark
                )
            ";

            using var conn = Conn();
            var affected = conn.Execute(sqlstr, mas);

            return affected > 0;

        }

        public bool Delete(int rowid)
        {
            string sqlstr = @"
                DELETE FROM z_org_user
                WHERE rowid = @rowid
            ";

            using var conn = Conn();
            int affected = conn.Execute(sqlstr, new { rowid });
            return affected > 0;

        }

        public z_org_user? GetById(int rowid)
        {
            string sqlstr = @"
                    SELECT 
                    rowid,
                    code_user,
                    is_enabled,
                    mno,
                    mname,
                    pass_user,
                    tel_user,
                    mobile_user,
                    email_user,
                    remark
                FROM z_org_user
                WHERE rowid = @rowid
            ";
            using var conn = Conn();
            return conn.QueryFirstOrDefault<z_org_user>(sqlstr, new { rowid });

        }

        public List<z_org_user> List(string code_user)
        {
            string sqlstr = @"
                SELECT 
                    rowid,
                    code_user,
                    is_enabled,
                    mno,
                    mname,
                    pass_user,
                    tel_user,
                    mobile_user,
                    email_user,
                    remark
                FROM z_org_user
                where code_user=@code_user
                ORDER BY mno            
            ";

            using var conn = Conn();
            return conn.Query<z_org_user>(sqlstr, new { code_user }).ToList();

        }

        public z_org_user? Login(string userId, string password)
        {
            string sqlstr = @"
            SELECT
            rowid,
            code_user,
            mno,
            mname
            FROM z_org_user
            WHERE mno = @userId
            AND pass_user = @password
            AND is_enabled = 1
            ";

            using var conn = Conn();

            return conn.QueryFirstOrDefault<z_org_user>(sqlstr, new
            {
                userId,
                password
            });


        }

        public bool Update(z_org_user mas)
        {
            string sqlstr=@"
                UPDATE z_org_user
                SET
                    is_enabled = @is_enabled,
                    mno = @mno,
                    mname = @mname,
                    tel_user = @tel_user,
                    mobile_user = @mobile_user,
                    email_user = @email_user,
                    remark = @remark
                WHERE rowid = @rowid
            ";

            using var conn=Conn();
            var affected=conn.Execute(sqlstr,mas);
         
            return affected>0;
        }

        private IDbConnection Conn()
        {
            return new SqlConnection(_config.GetConnectionString("mvcstore"));
        }

    }
}