using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class HandwrittenMapper
    {
        public static BenchDestination Map(BenchSource src, BenchDestination dest)
        {
            if (src == null)
            {
                return null;
            }
            if (dest == null)
            {
                dest = new BenchDestination();
            }
            dest.i1 = Map(src.i1, dest.i1);
            dest.i2 = Map(src.i2, dest.i2);
            dest.i3 = Map(src.i3, dest.i3);
            dest.i4 = Map(src.i4, dest.i4);
            dest.i5 = Map(src.i5, dest.i5);
            dest.i6 = Map(src.i6, dest.i6);
            dest.i7 = Map(src.i7, dest.i7);
            dest.i8 = Map(src.i8, dest.i8);

            dest.n2 = src.n2;
            dest.n3 = src.n3;
            dest.n4 = src.n4;
            dest.n5 = src.n5;
            dest.n6 = src.n6;
            dest.n7 = src.n7;
            dest.n8 = src.n8;
            dest.n9 = src.n9;

            dest.s1 = src.s1;
            dest.s2 = src.s2;
            dest.s3 = src.s3;
            dest.s4 = src.s4;
            dest.s5 = src.s5;
            dest.s6 = src.s6;
            dest.s7 = src.s7;

            return dest;
        }

        public static BenchDestination.Int1 Map(BenchSource.Int1 src, BenchDestination.Int1 dest)
        {
            if (src == null)
            {
                return null;
            }
            if (dest == null)
            {
                dest = new BenchDestination.Int1();
            }

            dest.i = src.i;
            dest.str1 = src.str1;
            dest.str2 = src.str2;

            return dest;
        }

        public static BenchDestination.Int2 Map(BenchSource.Int2 src, BenchDestination.Int2 dest)
        {
            if (src == null)
            {
                return null;
            }

            if (dest == null)
            {
                dest = new BenchDestination.Int2();
            }
            dest.i1 = Map(src.i1, dest.i1);
            dest.i2 = Map(src.i2, dest.i2);
            dest.i3 = Map(src.i3, dest.i3);
            dest.i4 = Map(src.i4, dest.i4);
            dest.i5 = Map(src.i5, dest.i5);
            dest.i6 = Map(src.i6, dest.i6);
            dest.i7 = Map(src.i7, dest.i7);

            return dest;
        }
    }
}
