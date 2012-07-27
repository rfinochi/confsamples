using System;
using System.Diagnostics;
using System.Linq;

namespace Continuations
{
    static class Extensions
    {
        public static int LengthLinear<T>(this IImmutableList<T> list)
        {
            return list.IsEmpty ? 0 : 1 + LengthLinear(list.Tail);
        }

        public static int LengthTail<T>(this IImmutableList<T> list)
        {
            Func<IImmutableList<T>, int, int> length_acc = null;
            length_acc = (l, acc) => l.IsEmpty ? acc : length_acc(l.Tail, 1 + acc);

            return length_acc(list, 0);
        }

        public static int LengthCPS<T>(this IImmutableList<T> list)
        {
            Func<IImmutableList<T>, Func<int, int>, int> length_cont = null;
            length_cont = (l, cont) => l.IsEmpty ? cont(0) : length_cont(l.Tail, x => cont(x + 1));

            return length_cont(list, x => x);
        }
    }

    class Program
    {
        static int FibonacciLinear(int n)
        {
            return n <= 2 ? 1 : FibonacciLinear(n - 2) + FibonacciLinear(n - 1);
        }

        static int FibonacciTail(int n)
        {
            Func<int, int, int, int> fibonacci_acc = null;
            fibonacci_acc = (x, next, result) => x <= 0 ? result : fibonacci_acc(x - 1, next + result, next);

            return fibonacci_acc(n, 1, 0);
        }

        static int FibonacciCPS(int n)
        {
            Func<int, Func<int, int>, int> fibonacci_cont = null;
            fibonacci_cont = (a, cont) => a <= 2 ? cont(1) : fibonacci_cont(a - 2, x => fibonacci_cont(a - 1, y => cont(x + y)));

            return fibonacci_cont(n, x => x);
        }

        static int FactorialLinear(int n)
        {
            return n <= 0 ? 1 : n * FactorialLinear(n - 1);
        }

        static int FactorialTail(int n)
        {
            Func<int, int, int> factorial_acc = null;
            factorial_acc = (x, acc) => x <= 0 ? acc : factorial_acc(x - 1, acc * x);

            return factorial_acc(n, 1);
        }

        static int FactorialCPS(int n)
        {
            Func<int, Func<int, int>, int> factorial_cont = null;
            factorial_cont = (a, func) => a <= 0 ? func(1) : factorial_cont(a - 1, x => func(x * a));

            return factorial_cont(n, x => x);
        }

        static void Main(string[] args)
        {
            var watch = new Stopwatch();
            
            watch.Start();
            var res1 = FactorialLinear(10);
            watch.Stop();
            Console.WriteLine("Factorial Linear time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res1);
            watch.Reset();

            watch.Start();
            var res2 = FactorialTail(10);
            watch.Stop();
            Console.WriteLine("Factorial Tail Elapsed time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res2);
            watch.Reset();

            watch.Start();
            var res3 = FactorialCPS(10);
            watch.Stop();
            Console.WriteLine("Factorial CPS Elapsed time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res3);
            watch.Reset();

            watch.Start();
            var res4 = FibonacciLinear(10);
            watch.Stop();
            Console.WriteLine("Fibonacci Linear time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res4);
            watch.Reset();

            watch.Start();
            var res5 = FibonacciTail(10);
            watch.Stop();
            Console.WriteLine("Fibonacci Tail Elapsed time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res5);
            watch.Reset();

            watch.Start();
            var res6 = FibonacciCPS(10);
            watch.Stop();
            Console.WriteLine("Fibonacci CPS Elapsed time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res6);
            watch.Reset();

            var list = ImmutableList<int>.Create(Enumerable.Range(1, 100).GetEnumerator());

            watch.Start();
            var res7 = list.LengthLinear();
            watch.Stop();
            Console.WriteLine("List Length Linear time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res7);
            watch.Reset();

            watch.Start();
            var res8 = list.LengthTail();
            watch.Stop();
            Console.WriteLine("List Length Tail Elapsed time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res8);
            watch.Reset();

            watch.Start();
            var res9 = list.LengthCPS();
            watch.Stop();
            Console.WriteLine("List Length CPS Elapsed time: {0}", watch.ElapsedTicks);
            Console.WriteLine("Result: {0}", res9);
            watch.Reset();
        }
    }
}
