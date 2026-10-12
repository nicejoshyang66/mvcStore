

using mvc_Dapper.Models.Entity;
using mvc_Dapper.Models.Repo;

namespace mvc_Dapper.Models.Service
{
    public class ZSysProgramService:IZSysProgramService
    {
        private readonly IZSysProgramRepo _repo;

        public ZSysProgramService(IZSysProgramRepo repo)
        {
            _repo=repo;
        }

        public List<z_sys_program> GetMenuTree(string code_user)
        {
            var all=_repo.GetEnabled(code_user);

            var dict=new Dictionary<string,z_sys_program>();

            foreach (var item in all)
            {
                if (!string.IsNullOrWhiteSpace(item.mno))
                {
                    dict[item.mno]=item;
                }
            }

            var roots = new List<z_sys_program>();

            foreach (var item in all)
            {
                // 第一層：沒有 parentno，或找不到父層
                if (string.IsNullOrWhiteSpace(item.parentno) || !dict.ContainsKey(item.parentno))
                {
                    roots.Add(item);
                }
                else
                {
                    // 第二層：只掛到父層下面
                    dict[item.parentno].Children.Add(item);
                }
            }

            // 第一層排序
            roots = roots
                .OrderBy(x => x.no_sort)
                .ToList();

            // 第二層排序，不再往下遞迴
            foreach (var root in roots)
            {
                root.Children = root.Children
                    .OrderBy(x => x.no_sort)
                    .ToList();
            }

            return roots;

        }
    }

}