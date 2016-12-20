using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemo.LinqToSql
{
    public class LinqToSqlHelper
    {
        public static void Test1()
        {
            using (var context = new DefectModelDataContext())
            {
                context.Log = Console.Out;
                DefectUser zhangsan = context.DefectUser
                    .Where(user => user.UserID == "zhangsan")
                    .Single();
                var query = from defect in context.Defect
                            where defect.DefectStatus != "Closed"
                            where defect.AssignedTo == zhangsan.UserID
                            select defect.DefectSummary;
                foreach (var summary in query)
                {
                    Console.WriteLine(summary);
                }
            }
        }

        public static void Test2()
        {
            using (var context = new DefectModelDataContext())
            {
                context.Log = Console.Out;
                var query = from defect in context.Defect
                            join proj in context.Project
                              on defect.Project equals proj.ProjID
                            select new { defect.DefectSummary, proj.ProjName };
                foreach (var item in query)
                {
                    Console.WriteLine(string.Format("defect:{0},project:{1}", item.DefectSummary, item.ProjName));
                }
            }
        }
    }
}
