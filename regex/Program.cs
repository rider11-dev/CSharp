using System;
using System.Text.RegularExpressions;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string pattern=@"^((\d{3,4}\-?\d{7,8})|(\d{5}))$";//电话号码
            Console.WriteLine("reg string is :{0}",pattern);
            FindMatches(pattern);
            
            Console.WriteLine("press any key to exit:");
            Console.ReadKey();
        }

        static void FindMatches(string pattern)
        {
            while(true)
            {
                Console.WriteLine("enter your string(press exit to quit):");
                var str=Console.ReadLine();
                if(str=="exit")
                {
                    break;
                }
                var matches=Regex.Matches(str,pattern);
                if(matches!=null&&matches.Count>0)
                {
                    Console.WriteLine("matches found:");
                    foreach(var match in matches)
                    {
                        Console.Write(match.ToString());
                        Console.Write(" | ");
                    }
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("matches not found!");
                }
            }
        }
    }
}
