
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Repo
{
    public class ZSysProgramRepo : IZSysProgramRepo
    {
        private readonly string? _connstr;

        public ZSysProgramRepo(IConfiguration configuration)
        {
            _connstr = configuration.GetConnectionString("mvcstore");
        }

        public bool AddMas(z_sys_program mas)
        {

            string sqlstr = @"
            insert into z_sys_program
            (is_enabled,code_no,no_sort,mno, mname,remark)
            values 
            (@is_enabled,@code_no,@no_sort,@mno, @mname,@remark)";

            using var conn = CreateConn();

            var affected = conn.Execute(sqlstr, mas);
            return affected > 0;

        }

        public bool AddDet(z_sys_program det)
        {

            string sqlstr = @"
            insert into z_sys_program
            (is_enabled, mno, mname, code_no,no_sort,controllername,actionname, parentno, remark, routename)
            values
            (@is_enabled,@mno,@mname, @code_no,@no_sort,@controllername,@actionname,@parentno,@remark, @routename)";

            using var conn = CreateConn();
            var affected = conn.Execute(sqlstr, det);

            return affected > 0;

        }

        public bool DeleteMas(string id)
        {
            string sqlstr = @"
            delete from z_sys_program
            where mno=@id
            
            delete from z_sys_program
            where parentno=@id
            ";
            using var conn = CreateConn();
            var affected = conn.Execute(sqlstr, new { id });
            return affected > 0;

        }

        public List<z_sys_program> GetEnabled(string code_user)
        {
            string sqlstr = @"
            select rowid,mno,mname, parentno, is_enabled,code_no,type_no, no_sort,
            controllername,actionname,routename
            from z_sys_program
            where is_enabled=1
            and code_no= @code_user
            order by no_sort
            ";

            using var conn = CreateConn();

            // var mas = conn.Query<z_sys_program>(sqlstr, new{code_user}).ToList();

            var mas = conn.Query<z_sys_program>(sqlstr, new { code_user }).ToList();

            return mas;
        }


        public z_sys_program? GetMas(string id)
        {

            string @sqlstr = @"
            select rowid,mno,mname, parentno, is_enabled,type_no,no_sort,
            case when code_no='u' then '一般使用者'
                 when code_no='m' then '資訊管理員'
                 end code_name,                
            controllername,actionname, remark, code_no
            from z_sys_program
            where mno=@id";

            using var conn = CreateConn();
            // z_sys_program? mas = conn.QueryFirstOrDefault<z_sys_program>(sqlstr, new { id });

            z_sys_program? mas = conn.QueryFirstOrDefault<z_sys_program>(sqlstr, new { id });

            return mas;

        }

        public z_sys_program? GetDet(int id)
        {
            string @sqlstr = @"
            select rowid,mno,mname, parentno, is_enabled,type_no,no_sort,
            case when code_no='u' then '一般使用者'
                 when code_no='m' then '資訊管理員'
                 end code_name,                
            controllername,actionname, remark, code_no, routename
            from z_sys_program
            where rowid=@id";

            using var conn = CreateConn();
            z_sys_program? det = conn.QueryFirstOrDefault<z_sys_program>(sqlstr, new { id });

            return det;
        }

        public List<z_sys_program> GetDetList(string parentno)
        {
            string sqlstr = @"
            select rowid,mno,mname,parentno,is_enabled,type_no,no_sort,
            controllername,actionname,remark,
            case when code_no='u' then '一般使用者'
                when code_no='m' then '資訊管理員'
                else code_no end as code_no, routename
            from z_sys_program
            where parentno = @parentno
            order by no_sort, mno
            ";

            using var conn = CreateConn();
            // return conn.Query<z_sys_program>(sqlstr, new { parentno }).ToList();

            List<z_sys_program> det = conn.Query<z_sys_program>(sqlstr, new { parentno }).ToList();

            return det;

            // return conn.Query<z_sys_program>(sqlstr,new{parentno}).ToList();                

        }


        public List<z_sys_program> GetMasList()
        {
            string sqlstr = @"
            select rowid,mno,mname,parentno,is_enabled,type_no,no_sort,
                controllername,actionname,remark,code_no,
                case when code_no='u' then '一般使用者' 
                        when code_no='m' then '資訊管理員'
                        else code_no end as code_name
            from z_sys_program
            where 1=1
            and (parentno is null or parentno = '')
            order by code_no, no_sort, mno
            ";

            using var conn = CreateConn();
            // return conn.Query<z_sys_program>(sqlstr).ToList();

            List<z_sys_program> mas = conn.Query<z_sys_program>(sqlstr).ToList();

            return mas;

        }

        public bool UpdateMas(z_sys_program mas)
        {
            string sqlstr = @"
            update z_sys_program set 
            mno=@mno, mname=@mname, is_enabled=@is_enabled,no_sort=@no_sort,
            remark=@remark, code_no = @code_no
            where rowid=@rowid";
            using var conn = CreateConn();
            var affected = conn.Execute(sqlstr, mas);
            return affected > 0;
        }

        private IDbConnection CreateConn()
        {
            return new SqlConnection(_connstr);
        }

        public bool UpdateDet(z_sys_program det)
        {
            string sqlstr = @"
            update z_sys_program set 
            is_enabled=@is_enabled,
            mno=@mno, mname=@mname,no_sort=@no_sort,
            parentno=@parentno, code_no = @code_no,
            controllername=@controllername, actionname=@actionname,
            remark=@remark,
            routename=@routename
            where rowid=@rowid";
            using var conn = CreateConn();
            var affected = conn.Execute(sqlstr, det);
            return affected > 0;
        }

        public bool DeleteDet(int id)
        {
            string sqlstr = @"
            delete from z_sys_program
            where rowid=@id
            ";
            using var conn = CreateConn();
            var affected = conn.Execute(sqlstr, new { id });
            return affected > 0;
        }
    }
}