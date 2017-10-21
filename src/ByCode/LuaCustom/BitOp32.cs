using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuaCustom
{
    public class BitOp32
    {
        public static int And(int v1, int v2)
        {
            return v1 & v2;
        }

        public static int Or(int v1, int v2)
        {
            return v1 | v2;
        }

        public static int Not(int v)
        {
            return ~v;
        }

        public static int RShift(int v, int step)
        {
            return v >> step;
        }

        public static int LShift(int v, int step)
        {
            return v << step;
        }

        public static int Get(int lval, int rval)
        {
            return (lval & (1 << rval)) != 0 ? 1 : 0;
        }

        public static int Set(int lval, int rval)
        {
            return lval | (1 << rval);
        }

        public static int Clear(int lval, int rval)
        {
            return lval & ~(1 << rval);
        }
    }
}
