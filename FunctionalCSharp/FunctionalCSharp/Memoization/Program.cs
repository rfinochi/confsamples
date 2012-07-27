using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Memoization
{
public class Table<T, U> : IDisposable
{
    private readonly Dictionary<T, U> dictionary;
    private readonly Func<T, U> func;

    public Table(Dictionary<T, U> dictionary, Func<T, U> func)
    {
        this.dictionary = dictionary;
        this.func = func;
    }

    public U this[T n]
    {
        get
        {
            if (dictionary.ContainsKey(n))
                return dictionary[n];

            var result = func(n);
            dictionary.Add(n, result);
            return result;
        }
    } 

    public void Dispose()
    {
        dictionary.Clear();
    }
}

    static class Extensions
    {
        // Memoize Function
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        {
            var t = new Dictionary<T, TResult>();
            return n =>
            {
                if (t.ContainsKey(n)) return t[n];
                else
                {
                    var result = func(n);
                    t.Add(n, result);
                    Console.WriteLine("{0} - {1}", n, result);
                    return result;
                }
            };
        }
    }

    class Program
    {
        static Func<int, int> FibonacciFast
        {
            get
            {
                var t = new Dictionary<int, int>();
                Func<int, int> fibCached = null;
                fibCached = n =>
                {
                    if (t.ContainsKey(n)) return t[n];
                    if (n <= 2) return 1;
                    
                    var result = fibCached(n - 2) + fibCached(n - 1);
                    t.Add(n, result);
                    return result;
                };

                return n => fibCached(n); 
            }
        }

        static Func<int, int> FibonacciMemoized
        {
            get
            {
                return Extensions.Memoize((int n) => n <= 2 ? 1 : Fibonacci(n - 2) + Fibonacci(n - 1));
            }
        }

        static int Fibonacci(int n)
        {
            return n <= 2 ? 1 : Fibonacci(n - 2) + Fibonacci(n - 1);
        }

        static Table<T, U> MemoizeTable<T, U>(Func<T, U> func)
        {
            return new Table<T, U>(new Dictionary<T, U>(), func);
        }

        static Table<int, int> FibonacciTable
        {
            get
            {
                return MemoizeTable((int n) => n <= 2 ? 1 : FibonacciTable[n - 2] + FibonacciTable[n - 1]);
            }
        }


        static void Main(string[] args)
        {
            // Memoized with function
            var watch = new Stopwatch();
            watch.Start();
            var fibonacciFunc = FibonacciMemoized;
            var fibFast1 = fibonacciFunc(30);
            watch.Stop();
            Console.WriteLine("Fibonacci Memoized Elapsed time: {0}", watch.ElapsedTicks);
            watch.Reset();

            watch.Start();
            var fibFast2 = fibonacciFunc(30);
            watch.Stop();
            Console.WriteLine("Fibonacci Memoized Elapsed time: {0}", watch.ElapsedTicks);
            watch.Reset();

            // Memoize Generic
            Func<int, int> fibonacci = Fibonacci;
            var fibonacciMemoized = fibonacci.Memoize();

            watch.Start();
            var fibFast3 = fibonacciMemoized(30);
            watch.Stop();
            Console.WriteLine("Fibonacci Memoized Extension Elapsed time: {0}", watch.ElapsedTicks);
            watch.Reset();

            watch.Start();
            var fibFast4 = fibonacciMemoized(30);
            watch.Stop();
            Console.WriteLine("Fibonacci Memoized Extension Elapsed time: {0}", watch.ElapsedTicks);
            watch.Reset();

            // Table approach
            watch.Start();
            var fibTable = FibonacciTable;
            var fibFast5 = fibTable[30];
            watch.Stop();
            Console.WriteLine("Fibonacci Table Elapsed time: {0}", watch.ElapsedTicks);
            watch.Reset();

            watch.Start();
            var fibFast6 = fibTable[30];
            watch.Stop();
            Console.WriteLine("Fibonacci Table Elapsed time: {0}", watch.ElapsedTicks);
            watch.Reset();

            // Memoize by FibFast
            watch.Start();
            var fibFast = FibonacciFast;
            var fibFast7 = fibFast(30);
            watch.Stop();
            Console.WriteLine("Fibonacci Fast Elapsed time: {0}", watch.ElapsedTicks);
            watch.Reset();

            watch.Start();
            var fibFast8 = fibFast(30);
            watch.Stop();
            Console.WriteLine("Fibonacci Fast Elapsed time: {0}", watch.ElapsedTicks);
            watch.Reset();
        }
    }
}
