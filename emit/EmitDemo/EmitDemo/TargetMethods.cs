using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitDemo
{
    public class TargetMethods
    {
        public class Fibonacci
        {
            public int Calc(int num)
            {
                if (num == 1 || num == 2)
                {
                    return 1;
                }
                else
                {
                    return Calc(num - 1) + Calc(num - 2);
                }
            }
        }

        public class Add
        {
            private int _a = 0;

            public int A

            {

                get { return _a; }

                set { _a = value; }

            }



            private int _b = 0;

            public int B

            {

                get { return _b; }

                set { _b = value; }

            }

            public Add(int a, int b)

            {

                _a = a;

                _b = b;

            }



            public int Calc()

            {

                return _a + _b;

            }
        }

        public class Iterator
        {
            public int ForMethod(int[] ints)
            {
                int sum = 0;
                for (int i = 0; i < ints.Length; i++)
                {
                    sum += ints[i];
                }
                return sum;
            }

            public int ForeachMethod(int[] ints)
            {
                int sum = 0;
                foreach (int i in ints)
                {
                    sum += i;
                }
                return sum;
            }
        }

        public class ExceptionHandler
        {
            public static int ConvertToInt32(string str)
            {
                int num = 0;
                try
                {
                    num = Convert.ToInt32(str);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return num;
            }
        }
    }
}
