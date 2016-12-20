using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public class RoleExt : Role
    {
        public new int Id { get; set; }
        public int RoleID { get; set; }
    }
}
