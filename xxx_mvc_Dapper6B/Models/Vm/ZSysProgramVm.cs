
using mvc_Dapper.Models.Entity;

namespace mvc_Dapper.Models.Vm
{
    public class ZSysProgramVm
    {
        public z_sys_program? Mas{get;set;}=new();
        public List<z_sys_program> DetList{get;set;}=new List<z_sys_program>();

    }
}