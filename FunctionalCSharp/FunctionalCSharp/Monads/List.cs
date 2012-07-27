using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monads
{
    public static class ListExtensions
    {
        public static IEnumerable<U> SelectMany<T, U>(this IEnumerable<T> m, Func<T, IEnumerable<U>> k)
        {
            foreach (var x in m)
                foreach (var y in k(x))
                    yield return y;
        }
    }

    public class ListMonad
    {
        public static void LinqExample()
        {
            var r = from x in Enumerable.Range(0, 3)
                    from y in Enumerable.Range(0, 3)
                    select x + y;

            foreach (var i in r) Console.WriteLine(i);
        }
    }
}
