using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using mvc_Dapper.Models.ViewModel;

namespace mvc_Dapper.Models.Repository
{
    public class ZSysSecurityRepo : IZSysSecurityRepo
    {
        private readonly string _connStr;

        public ZSysSecurityRepo(IConfiguration config)
        {
            _connStr = config.GetConnectionString("mvcstore")!;
        }

        private IDbConnection Conn()
        {
            return new SqlConnection(_connStr);
        }

        public List<GroupSecurityMasterVM> GetGroups()
        {
            string sqlstr = @"
                select mno, mname
                from z_sys_group
                where is_enabled = 1
                order by mno
            ";

            using var conn = Conn();
            var data = conn.Query<GroupSecurityMasterVM>(sqlstr);
            return data.ToList();
        }

        public List<GroupSecurityMasterVM> GetSourceGroups(string targetNo)
        {
            string sqlstr = @"
                select mno, mname
                from z_sys_group
                where is_enabled = 1
                  and mno <> @targetNo
                order by mno
            ";

            using var conn = Conn();
            var data = conn.Query<GroupSecurityMasterVM>(sqlstr, new { targetNo });
            return data.ToList();
        }

        public List<GroupSecurityDetailVM> GetSecurityByGroup(string mno)
        {
            string sqlstr = @"
                select 
                    s.rowid,
                    s.target_no,
                    s.program_no,
                    m.mname as program_name,
                    s.is_add,
                    s.is_edit,
                    s.is_delete,
                    s.is_confirm,
                    s.remark
                from z_sys_security s
                left join z_sys_program m on s.program_no = m.mno
                where s.target_no = @mno
                order by s.program_no
            ";

            using var conn = Conn();

            var data = conn.Query<GroupSecurityDetailVM>(
                sqlstr,
                new { mno }
            );

            return data.ToList();
        }

        public void UpdateGroupSecurity(string targetNo,List<GroupSecurityDetailVM>details)
        {
            string sqlstr=@"
            update z_sys_security
            set
            is_add=@is_add,is_edit=@is_edit,is_delete=@is_delete,is_confirm=@is_confirm,
            remark=@remark
            where rowid=@rowid
            ";

            using var conn=Conn();
            
            foreach (var item in details)
            {

                conn.Execute(sqlstr, new
                {
                    rowid=item.rowid,
                    is_add=item.is_add,
                    is_edit=item.is_edit,
                    is_delete=item.is_delete,
                    is_confirm=item.is_confirm,
                    remark=item.remark
                });
                
            }

            
        }


        public void CopyGroupSecurity(string targetNo, string sourceNo)
        {
            string deleteSql=@"
                delete from z_sys_security
                where target_no = @targetNo
            ";

            string insertSql=@"
                insert into z_sys_security
                (
                    type_no,
                    target_no,
                    program_no,
                    is_add,
                    is_edit,
                    is_delete,
                    is_confirm,
                    remark
                )
                select
                    type_no,
                    @targetNo as target_no,
                    program_no,
                    is_add,
                    is_edit,
                    is_delete,
                    is_confirm,
                    remark
                from z_sys_security
                where target_no = @sourceNo
            ";

            using var conn=new SqlConnection(_connStr);
            conn.Open();

            using var tran=conn.BeginTransaction();

            try
            {
                conn.Execute(deleteSql,new{targetNo},tran);
                conn.Execute(insertSql,new{targetNo,sourceNo},tran);
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}