using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LazyEvaluation
{
    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
    }

    class Program
    {
        static bool IsPrime(int number)
        {
            var isPrime = true;

            for (var i = 2; i <= Math.Sqrt(number) && isPrime; i++)
                isPrime = number % i != 0;

            return isPrime;
        }

        static IEnumerable<int> GetPrimes()
        {
            var index = 1;
            while (true)
            {
                if (IsPrime(index))
                    yield return index;
                index++;
            }
        }

        static IEnumerable<ulong> Fibonacci
        {
            get
            {
                ulong i = 1;
                ulong j = 1;
                while (true)
                {
                    yield return i;
                    var temp = i;
                    i = j;
                    j = j + temp;
                }
            }
        }

        // Lazy Recursion
        public static IEnumerable<string> GetFiles(string rootPath)
        {
            foreach (var file in Directory.GetFiles(rootPath))
                yield return file;
            foreach (var directory in Directory.GetDirectories(rootPath))
                foreach (var file in GetFiles(directory))
                    yield return file;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("First 100 Prime numbers");
            var primes100 = GetPrimes().Take(100);
            primes100.ForEach(Console.WriteLine);

            Console.WriteLine("First 100 Fibonacci numbers");
            var fibonacci100 = Fibonacci.Take(100);
            fibonacci100.ForEach(Console.WriteLine);

            Console.WriteLine("Files in Directory");
            var files = GetFiles(Environment.CurrentDirectory);
            files.ForEach(Console.WriteLine);
        }
    }
}
