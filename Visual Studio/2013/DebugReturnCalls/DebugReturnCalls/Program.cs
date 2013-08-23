using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReturnCalls
{
    class Program
    {
        static void Main(string[] args)
        {
            Method1();     
            int y = Method2(); 
        }

        static void Method1()
        {
            int result = Multiply(FourTimes(Five()), Six());
        }

        static int Method2()
        {
            return Five();
        }

        static int Multiply(int x, int y)
        {
            return x * y;
        }

        static int FourTimes(int x)
        {
            return 4 * x;
        }

        static int Five()
        {
            return 5;
        }

        static int Six()
        {
            return 6;
        }
    }
}
