using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repository
{
    public class ZSysGroupRepo : IZSysGroupRepo
    {
        private readonly string _connStr;

        public ZSysGroupRepo(IConfiguration config)
        {
            _connStr = config.GetConnectionString("mvcstore")!;
        }

        private IDbConnection Conn()
        {
            return new SqlConnection(_connStr);
        }

        public List<z_sys_group> List()
        {
            string sql = @"
                SELECT rowid, is_enabled, mno, mname, remark
                FROM z_sys_group
                ORDER BY mno
            ";

            using var conn = Conn();
            return conn.Query<z_sys_group>(sql).ToList();
        }

        public z_sys_group? GetByRowid(int rowid)
        {
            string sql = @"
                SELECT rowid, is_enabled, mno, mname, remark
                FROM z_sys_group
                WHERE rowid = @rowid
            ";

            using var conn = Conn();
            return conn.QueryFirstOrDefault<z_sys_group>(sql, new { rowid });
        }

        public z_sys_group? GetByMno(string mno)
        {
            string sql = @"
                SELECT rowid, is_enabled, mno, mname, remark
                FROM z_sys_group
                WHERE mno = @mno
            ";

            using var conn = Conn();
            return conn.QueryFirstOrDefault<z_sys_group>(sql, new { mno });
        }

        public List<z_org_user_group> ListGroupUsers(string groupNo)
        {
            string sql = @"
                SELECT 
                    z_org_user_group.rowid,
                    z_org_user_group.user_no,
                    z_org_user.mname,
                    z_org_user_group.group_no,
                   /* z_org_user_group.code_user,*/
                    CAST(1 AS bit) AS is_checked
                FROM z_org_user_group
                LEFT JOIN z_org_user 
                    ON z_org_user_group.user_no = z_org_user.mno
                WHERE z_org_user_group.group_no = @groupNo
                ORDER BY z_org_user_group.user_no
            ";

            using var conn = Conn();
            return conn.Query<z_org_user_group>(sql, new { groupNo }).ToList();
        }

        public List<z_org_user_group> ListAllUsersForGroup(string groupNo)
        {
            string sql = @"
                SELECT 
                    z_org_user.rowid,
                    z_org_user.mno AS user_no,
                    z_org_user.mname,
                    @groupNo AS group_no,
                  /*  z_org_user.code_user,*/
                    CASE 
                        WHEN z_org_user_group.user_no IS NULL THEN CAST(0 AS bit)
                        ELSE CAST(1 AS bit)
                    END AS is_checked
                FROM z_org_user
                LEFT JOIN z_org_user_group
                    ON z_org_user_group.user_no = z_org_user.mno
                    AND z_org_user_group.group_no = @groupNo
                ORDER BY z_org_user.mno
            ";

            using var conn = Conn();
            return conn.Query<z_org_user_group>(sql, new { groupNo }).ToList();
        }

        public int UpdateGroupUsers(string groupNo, List<string>? selectedUserNo)
        {
            using var conn = new SqlConnection(_connStr);
            conn.Open();

            using var tran = conn.BeginTransaction();

            try
            {
                string deleteSql = @"
                    DELETE FROM z_org_user_group
                    WHERE group_no = @groupNo
                ";

                conn.Execute(deleteSql, new { groupNo }, tran);

                int count = 0;

                if (selectedUserNo != null)
                {
                    string insertSql = @"
                        INSERT INTO z_org_user_group
                        (
                            user_no,
                            group_no
                         /*   code_user*/
                        )
                        SELECT 
                            @user_no,
                            @groupNo
                          /*  code_user */
                        FROM z_org_user
                        WHERE mno = @user_no
                    ";

                    foreach (string userNo in selectedUserNo)
                    {
                        count += conn.Execute(
                            insertSql,
                            new
                            {
                                user_no = userNo,
                                groupNo
                            },
                            tran
                        );
                    }
                }

                tran.Commit();
                return count;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public int Create(z_sys_group model)
        {
            string sql = @"
                INSERT INTO z_sys_group
                (
                    is_enabled,
                    mno,
                    mname,
                    remark
                )
                VALUES
                (
                    @is_enabled,
                    @mno,
                    @mname,
                    @remark
                )
            ";

            using var conn = Conn();
            return conn.Execute(sql, model);
        }

        public int Edit(z_sys_group model)
        {
            string sql = @"
                UPDATE z_sys_group
                SET
                    is_enabled = @is_enabled,
                    mno = @mno,
                    mname = @mname,
                    remark = @remark
                WHERE rowid = @rowid
            ";

            using var conn = Conn();
            return conn.Execute(sql, model);
        }

        public int Delete(int rowid)
        {
            string sql = @"
                DELETE FROM z_sys_security
                WHERE target_no IN
                (
                    SELECT mno 
                    FROM z_sys_group 
                    WHERE rowid = @rowid
                )

                DELETE FROM z_org_user_group
                WHERE group_no IN
                (
                    SELECT mno
                    FROM z_sys_group
                    WHERE rowid = @rowid
                )

                DELETE FROM z_sys_group
                WHERE rowid = @rowid
            ";

            using var conn = Conn();
            return conn.Execute(sql, new { rowid });
        }
    }
}