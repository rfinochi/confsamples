using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Impure
{
    class Program
    {
        static bool DivisibleByTwo(int x)
        {
            Console.Write("{0} divisible by 2?;", x); return x % 2 == 0;
        }

        static bool DivisibleByThree(int x)
        {
            Console.Write("{0} divisible by 3?;", x); return x % 3 == 0;
        }

        static void OrderOfEffects()
        {
            var q0 = from x in Enumerable.Range(1, 100)
                     where DivisibleByTwo(x)
                     select x;
            var q1 = from x in q0
                     where DivisibleByThree(x)
                     select x;

            foreach (var r in q1) Console.WriteLine("[{0}];", r);
        }

        static void CatchingExceptions()
        {
            var xs = new[]{ 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };
            IEnumerable<int> q;

            try
            {
                q = from x in xs select 1/x;
            }
            catch
            {
                q = new int[0];
            }

            foreach (var z in q)
                Console.WriteLine(z);
        }

        static void ManagingResources()
        {
            Func<string> GetContents;
            using (var file = File.OpenText("file.txt"))
                GetContents = file.ReadToEnd;

            // SURPRISE!
            Console.WriteLine(GetContents());
        }

        static void LawOfClosures()
        {
            var DelayedActions = new List<Func<int>>();
            var s = "";

            for (var i = 4; i < 7; i++)
            {
                DelayedActions.Add(() => i);
            }
            for (var k = 0; k < DelayedActions.Count; k++)
                s += DelayedActions[k]();

            Console.WriteLine(s);
        }

        static void LawOfClosuresCopied()
        {
            var DelayedActions = new List<Func<int>>();
            var s = "";

            for (var i = 4; i < 7; i++)
            {
                var j = i;
                DelayedActions.Add(() => j);
            }

            for (var k = 0; k < DelayedActions.Count; k++)
                s += DelayedActions[k]();

            Console.WriteLine(s);
        }

        static void Main(string[] args)
        {
            OrderOfEffects();

            CatchingExceptions();

            ManagingResources();

            LawOfClosures();

            LawOfClosuresCopied();
        }
    }
}
