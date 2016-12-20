using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class UserExt : User
    {
        public UserExt()
        {
            Roles = new List<RoleExt>();
        }

        public List<RoleExt> Roles { get; set; }

        public override string ToString()
        {
            string roles = string.Empty;
            if (Roles != null && Roles.Count > 0)
            {
                roles = string.Join(",", Roles.Select(r => r == null ? "" : r.RoleName));
            }
            string str = string.Format("UserId:{0},UserName:{1},Email:{2},LastModifyTime:{3},Enabled:{4},Roles:{5}"
                , base.Id, base.UserName, base.Email, base.LastModifyTime.ToString(), base.Enabled, roles);

            return str;
        }
    }
}
