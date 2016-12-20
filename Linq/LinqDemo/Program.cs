using LinqDemo.LinqToSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //LinqToSqlHelper.Test1();
            //LinqToSqlHelper.Test2();
            LinqCartesian.Do();

            Console.ReadKey();
        }
    }
}
