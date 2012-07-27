using System;

namespace Monads
{
    public delegate Answer K<T, Answer>(Func<T, Answer> k);

    public static class ContinuationExtensions
    {
        public static K<V, Answer> SelectMany<T, U, V, Answer>(this K<T, Answer> m, Func<T, K<U, Answer>> k, Func<T, U, V> s)
        {
            return m.SelectMany(x => k(x).SelectMany(y => s(x, y).ToContinuation<V, Answer>()));
        }

        public static K<U, Answer> SelectMany<T, U, Answer>(this K<T, Answer> m, Func<T, K<U, Answer>> k)
        {
            return c => m(x => k(x)(c));
        }

        public static K<T, Answer> ToContinuation<T, Answer>(this T value)
        {
            return c => c(value);
        }
    }

    public class ContinuationMonad
    {
        public static void LinqExample()
        {
            var r = from x in 7.ToContinuation<int, string>()
                    from y in 6.ToContinuation<int, string>()
                    select x + y;

            Console.WriteLine(r(x => x.ToString().Replace('1', 'z')));
        }
    }
}
