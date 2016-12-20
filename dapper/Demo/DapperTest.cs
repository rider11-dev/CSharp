using MyFrame.RBAC.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Demo
{
    public class DapperTest
    {
        static string tb_user = "Users";
        static string tb_rel_user_role = "UserRoleRelation";
        static string tb_role = "Roles";
        public static void OneToMany()
        {
            List<UserExt> users = new List<UserExt>();
            using (IDbConnection conn = DbConnectionProvider.Create())
            {
                //conn.Open();

                //string sqlUpdate = string.Format("update {0} set LastModifyTime=GETDATE()", tb_user);
                //conn.Query(sqlUpdate);

                string sql = string.Format(@"select usr.Id,usr.UserName,usr.Email,usr.LastModifyTime,usr.Enabled,role.RoleName,rel.RoleId 
from {0} usr with(nolock) 
left join {1} rel on usr.Id=rel.UserID 
left join {2} role on rel.RoleID=role.Id", tb_user, tb_rel_user_role, tb_role);

                users = conn.Query<UserExt, RoleExt, UserExt>(sql,
                    (usr, role) =>
                    {
                        usr.Roles.Add(role);
                        //Console.WriteLine("usr:{0},role:{1}", usr.UserName, role.RoleName);
                        return usr;
                    }, null, null, false,"RoleId,RoleName").ToList();
                if (users.Count > 0)
                {
                    users.ForEach(usr => Console.WriteLine(usr.ToString()));
                }
            }
        }

        public static void TransactionTest()
        {
            using (UnitOfWork unit = new UnitOfWork(DbConnectionProvider.Create()))
            {
                unit.Begin();

                Action<IDbConnection, IDbTransaction> act = (connection, tran) =>
                {
                    string sqlUpdate = string.Format("update {0} set LastModifyTime=GETDATE()", tb_user);
                    connection.Query(sqlUpdate, null, tran);
                };
                unit.DoQuery(act);

                string sql = string.Format(@"select usr.Id,usr.UserName,usr.Email,usr.LastModifyTime,usr.Enabled,role.RoleName,rel.RoleId 
from {0} usr with(nolock) 
left join {1} rel on usr.Id=rel.UserID 
left join {2} role on rel.RoleID=role.Id", tb_user, tb_rel_user_role, tb_role);

                List<UserExt> users = new List<UserExt>();
                act = (connection, tran) =>
                {
                    users = connection.Query<UserExt, RoleExt, UserExt>(sql,
                    (usr, role) =>
                    {
                        usr.Roles.Add(role);
                        //Console.WriteLine("usr:{0},role:{1}", usr.UserName, role.RoleName);
                        return usr;
                    }, null, tran, false, "RoleId,RoleName").ToList();
                };

                unit.DoQuery(act);

                if (users.Count > 0)
                {
                    users.ForEach(usr => Console.WriteLine(usr.ToString()));
                }

                unit.Rollback();

            }
        }
    }
}
