using mvc_Dapper.Models.ViewModel;

namespace mvc_Dapper.Models.Repository
{
    public interface IZSysSecurityRepo
    {
        List<GroupSecurityMasterVM> GetGroups();

        List<GroupSecurityMasterVM> GetSourceGroups(string targetNo);

        List<GroupSecurityDetailVM> GetSecurityByGroup(string mno);

        void UpdateGroupSecurity(string targetNo, List<GroupSecurityDetailVM> details);

        void CopyGroupSecurity(string targetNo,string sourceNo);
    }
}