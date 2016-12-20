using EmitMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace test
{
    class Program
    {
        static string filePath = "dest.txt";
        static void Main(string[] args)
        {

            //Test();
            Test_EmitMapperResult();
            Console.WriteLine("press any key to exit:");
            Console.ReadKey();
        }

        static void Test()
        {
            int count = 1000;
            while (true)
            {
                Console.WriteLine("\r\nenter mapping count:");
                try
                {
                    string input = Console.ReadLine();
                    if (input == "exit")
                    {
                        break;
                    }
                    count = Convert.ToInt32(input);
                }
                catch { }
                Test_HandwrittenMapper(count);
                Test_EmitMapper(count);
                Test_AutoMapper(count);
            }
        }

        static void TestByKey()
        {
            int count = 1000;
            bool go_on = true;
            while (go_on)
            {
                Console.WriteLine("\r\nenter mapping count:");
                try
                {
                    count = Convert.ToInt32(Console.ReadLine());
                }
                catch { }
                Console.WriteLine("\r\nenter your cmd:");
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                Console.WriteLine();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.A:
                        Test_AutoMapper(count);
                        break;
                    case ConsoleKey.H:
                        Test_HandwrittenMapper(count);
                        break;
                    case ConsoleKey.E:
                        Test_EmitMapper(count);
                        break;
                    case ConsoleKey.X:
                        go_on = false;
                        break;
                    case ConsoleKey.C:
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine(" Unrecognized  command:" + keyInfo.Key.ToString());
                        break;
                }
            }
        }

        static void Test_HandwrittenMapper(int mappingsCount)
        {
            var src = new BenchSource();
            var dest = new BenchDestination();
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < mappingsCount; i++)
            {
                dest = HandwrittenMapper.Map(src, dest);
                //JsonConvert.SerializeObject(dest);
            }
            sw.Stop();

            PrintResult(mappingsCount, "Handwritten", sw.ElapsedMilliseconds);
        }

        static void Test_EmitMapper(int mappingsCount)
        {
            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<BenchSource, BenchDestination>();
            var src = new BenchSource();
            var dest = new BenchDestination();
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < mappingsCount; i++)
            {
                mapper.Map(src, dest);
            }
            sw.Stop();

            PrintResult(mappingsCount, "EmitMapper", sw.ElapsedMilliseconds);
        }

        static void Test_EmitMapperResult()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var mapper = ObjectMapperManager.DefaultInstance.GetMapper<BenchSource, BenchDestination>();
            var src = new BenchSource();
            Console.WriteLine("mapping and serializing...");
            for (int i = 100; i > 0; i--)
            {
                src.i1.i1.i = i;
                src.s1 = "s1_" + i;
                var dd = mapper.Map(src);
                SaveToFile(dd);
            }

            //
            Console.WriteLine("deserializing...");
            var dest = new BenchDestination();
            foreach (string str in File.ReadAllLines(filePath))
            {
                dest = JsonConvert.DeserializeObject<BenchDestination>(str);
                Console.WriteLine("dest.i1.i1.i:{0},dest.s1:{1}", dest.i1.i1.i, dest.s1);
            }

        }

        static void Test_AutoMapper(int mappingsCount)
        {
            AutoMapper.Mapper.CreateMap<BenchSource.Int1, BenchDestination.Int1>();
            AutoMapper.Mapper.CreateMap<BenchSource.Int2, BenchDestination.Int2>();
            AutoMapper.Mapper.CreateMap<BenchSource, BenchDestination>();

            var s = new BenchSource();
            var d = new BenchDestination();

            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < mappingsCount; ++i)
            {
                AutoMapper.Mapper.Map(s, d);
            }
            sw.Stop();

            PrintResult(mappingsCount, "AutoMapper", sw.ElapsedMilliseconds);
        }

        static void PrintResult(int count, string type, long timeSpan)
        {
            Console.WriteLine();
            Console.WriteLine("{0} times mapping,using {1},took {2}ms", count, type, timeSpan.ToString());
        }

        static void SaveToFile(BenchDestination dest)
        {
            var txt = JsonConvert.SerializeObject(dest);
            File.AppendAllText("dest.txt", txt + "\r\n");
        }
    }
}
