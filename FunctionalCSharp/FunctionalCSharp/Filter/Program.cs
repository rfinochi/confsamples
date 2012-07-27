using System;
using System.Collections.Generic;
using System.Linq;

namespace Filter
{
    static class SequenceExtensions
    {
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> items, Predicate<T> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");  
 
            foreach(var item in items)
            {
                if(predicate(item))
                    yield return item;
            }
        }

        // F# Seq.iter
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in items)
                action(item);
        }

        public static bool IsPrime(int number)
        {
            bool isPrime = true;

            for (int i = 2; i <= Math.Sqrt(number) && isPrime; i++)
                isPrime = number % i != 0;

            return isPrime;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var under100 = Enumerable.Range(1, 100);

            // Imperative isPrimes
            Func<IEnumerable<int>, IEnumerable<int>> filter = items =>
            {
                var primes = new List<int>();

                foreach(var item in items)
                {
                    if(SequenceExtensions.IsPrime(item))
                        primes.Add(item);
                }

                return primes;
            };
            var primesUnder100Imperative = filter(under100);
            primesUnder100Imperative.ForEach(Console.WriteLine);

            // F# - [1..100] |> List.filter(fun x -> isPrime x)
            var primesUnder100Filter = under100.Filter(SequenceExtensions.IsPrime);
            primesUnder100Filter.ForEach(Console.WriteLine);
            
            // Using Where
            var primesUnder100Where = under100.Where(SequenceExtensions.IsPrime);
            primesUnder100Where.ForEach(Console.WriteLine);
            
            // Using LINQ Expression
            var primesUnder100Expression = from x in under100
                                           where SequenceExtensions.IsPrime(x)
                                           select x;
            primesUnder100Expression.ForEach(Console.WriteLine);
        }
    }
}
