using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemo
{
    /// <summary>
    /// linq笛卡尔积
    /// </summary>
    public class LinqCartesian
    {
        static string[] usrIds = { "a", "b", "c" };
        static string[] roleIds = { "a", "b", "c" };

        public static void Do()
        {
            var query = from usr in usrIds
                        join role in roleIds on 1 equals 1
                        select new
                        {
                            u = usr,
                            r = role
                        };
            var items = query.ToList();
            foreach (var itm in items)
            {
                Console.WriteLine(string.Format("u:{0},r:{1}", itm.u, itm.r));
            }
        }
    }
}
