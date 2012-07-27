using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Map
{
    public class SequenceMapEnumerator<T, U, V> : IEnumerator<V>
    {
        private readonly IEnumerator<T> e1;
        private readonly IEnumerator<U> e2;
        private readonly Func<T, U, V> f;

        public SequenceMapEnumerator(IEnumerator<T> e1, IEnumerator<U> e2, Func<T, U, V> f)
        {
            this.e1 = e1;
            this.e2 = e2;
            this.f = f;
        }

        public V Current
        {
            get { return f(e1.Current, e2.Current); }
        }

        public void Dispose()
        {
            e1.Dispose();
            e2.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return f(e1.Current, e2.Current); }
        }

        public bool MoveNext()
        {
            var n1 = e1.MoveNext();
            var n2 = e2.MoveNext();
            if (n1 != n2)
                throw new InvalidOperationException("Inequal list lengths");
            return n1;
        }

        public void Reset()
        {
            e1.Reset();
            e2.Reset();
        }
    }

    static class SequenceExtensions
    {
        public static IEnumerable<Tuple<TArg1, TArg2>> Zip<TArg1, TArg2>(this IEnumerable<TArg1> arg1, IEnumerable<TArg2> arg2, Func<TArg1, TArg2, Tuple<TArg1, TArg2>> func)
        {
            return arg1.Map2(arg2, func);
        }

        public static IEnumerable<TResult> Map<T, TResult>(this IEnumerable<T> items, Func<T, TResult> func)
        {
            foreach (var item in items)
                yield return func(item);
        }

        public static IEnumerable<TResult> MapIndex<T, TResult>(this IEnumerable<T> items, Func<int, T, TResult> func)
        {
            var index = 0;
            foreach (var item in items)
            {
                yield return func(index, item);
                index++;
            } 
        }

        public static IEnumerable<TResult> Map2<TArg1, TArg2, TResult>(this IEnumerable<TArg1> arg1, IEnumerable<TArg2> arg2, Func<TArg1, TArg2, TResult> func)
        {
            var e1 = arg1.GetEnumerator();
            var e2 = arg2.GetEnumerator();
            var s = new SequenceMapEnumerator<TArg1, TArg2, TResult>(e1, e2, func);

            while (s.MoveNext())
                yield return s.Current;
        }

        // F# Seq.iter
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in items)
                action(item);
        }

        public static void ForIndex<T>(this IEnumerable<T> items, Action<int, T> action)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (action == null) throw new ArgumentNullException("action");

            var index = 0;
            foreach (var item in items)
            {
                action(index, item);
                index++;
            }
        }

        public static bool IsPrime(int number)
        {
            var isPrime = true;

            for (var i = 2; i <= Math.Sqrt(number) && isPrime; i++)
                isPrime = number % i != 0;

            return isPrime;
        }

        public static IEnumerable<int> GetPrimes()
        {
            var index = 1;
            while(true)
            {
                if(IsPrime(index))
                    yield return index;
                index++;
            }
        }
    }

    static class Program
    {
        static IEnumerable<int> MapPrimesLazy(IEnumerable<int> items)
        {
            foreach (var item in items)
                yield return (item * item * item);
        }

        static void Main(string[] args)
        {
            var primes = SequenceExtensions.GetPrimes().Take(20);

            // Imperative
            Func<IEnumerable<int>, IEnumerable<int>> mapPrimes = items =>
            {
                var primeResult = new List<int>();

                foreach(var item in items)
                    primeResult.Add(item*item*item);

                return primes;
            };
            var cubedPrimesImperative = mapPrimes(primes);
            cubedPrimesImperative.ForIndex((i, x) => Console.WriteLine("{0} - {1}", i, x));

            // Imperative - Lazy
            var cubedPrimesImperativeLazy = MapPrimesLazy(primes);
            cubedPrimesImperativeLazy.ForIndex((i, x) => Console.WriteLine("{0} - {1}", i, x));

            // F# - primes |> List.map(fun x -> x * x * x)

            // Using Map
            var cubedPrimes = primes.Map(x => x*x*x);
            cubedPrimes.ForIndex((i, x) => Console.WriteLine("{0} - {1}", i, x));
            
            // Using Select
            var cubedPrimesSelect = primes.Select(x => x*x*x);
            cubedPrimesSelect.ForIndex((i, x) => Console.WriteLine("{0} - {1}", i, x));

            // Using LINQ Expressions
            var cubedPrimesExpression = from x in primes
                    select x*x*x;
            cubedPrimesExpression.ForIndex((i, x) => Console.WriteLine("{0} - {1}", i, x));

            // F# - primes |> List.mapi(fun i x -> i * x)
            var primesTimesIndex = primes.MapIndex((i, x) => i*x);
            primesTimesIndex.ForIndex((i, x) => Console.WriteLine("{0} - {1}", i, x));

            // Seq.map2 (fun x y -> x * y) [1..10] [11..20]
            var e1 = Enumerable.Range(1, 10);
            var e2 = Enumerable.Range(11, 10);
            var e3 = e1.Map2(e2, (x, y) => x*y);
            e3.ForEach(Console.WriteLine);

            // Seq.zip (fun x y -> x, y) [1..10] [11..20]
            var zip = e1.Zip(e2, Tuple.New);
            zip.ForEach(x => Console.WriteLine("{0} - {1}", x.Item1, x.Item2));
        }
    }
}
