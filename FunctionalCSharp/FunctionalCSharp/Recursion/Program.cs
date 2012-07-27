using System;
using System.Collections.Generic;
using System.IO;

namespace Recursion
{
    static class Extensions
    {
        // F# Seq.iter
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in items)
                action(item);
        }
    }

    class Program
    {
// Imperative looping
public static int FactorialImperative(int n)
{
    var fact = 1;
    var i = n;

    while (i > 0)
    {
        fact = fact * i;
        i--;
    }

    return fact;
}

        // Linear recursion
public static int FactorialLinear(int n)
{
    if (n <= 1)
        return 1;
    
    return n <=1 ? 1 : n * FactorialLinear(n - 1);
}

        // Tail recursion
public static ulong FactorialTail(ulong n)
{
    Func<ulong, ulong, ulong> factorial_aux = null;
    factorial_aux = (acc, x) => x <= 0 ? acc : factorial_aux(x * acc, x - 1UL);

    return factorial_aux(1UL, n);
}

        // Binary recursion
        public static int FibonacciBinary(int n)
        {
            if (n <= 2) return 1;

            return FibonacciBinary(n - 2) + FibonacciBinary(n - 1);
        }

        // Refactored Tail Fibonacci
        public static int FibonacciTail(int n)
        {
            Func<int, int, int, int> fib_aux = null;
            fib_aux = (a, b, x) => x <= 1 ? b : fib_aux(b, a + b, x - 1);
            return fib_aux(0, 1, n);
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

        // Mutual Recursion
        // Hofstader Sequence
        // F#
        // let rec maleSequence n = if n = 0 then 0 else n - femaleSequence(maleSequence n - 1)
        //   and femaleSequence n = if n = 0 then 1 else n - maleSequence(femaleSequence n - 1)
        public static int MaleSequence(int n)
        {
            if (0 == n)
                return 0;

            return n - FemaleSequence(MaleSequence(n - 1));
        }

        public static int FemaleSequence(int n)
        {
            if (0 == n)
                return 1;

            return n - MaleSequence(FemaleSequence(n - 1));
        }

        static void Main(string[] args)
        {
            // Imperative looping
            var factorialImperative = FactorialImperative(10);
            Console.WriteLine("Factorial Imperative: {0}", factorialImperative);

            // Linear recursion
            var factorialLinear = FactorialLinear(10);
            Console.WriteLine("Factorial Linear: {0}", factorialLinear);

            // Tail recursion
            var factorialTail = FactorialTail(10);
            Console.WriteLine("Factorial Recursion: {0}", factorialTail);

            // Binary recursion
            var fibonacciBinary = FibonacciBinary(10);
            Console.WriteLine("Fibonacci Binary: {0}", fibonacciBinary);

            // Refactored Fibonacci
            var fibonacciRefactored = FibonacciTail(10);
            Console.WriteLine("Fibonacci Tail: {0}", fibonacciRefactored);

            // Lazy Recursion
            var filesUnderDir = GetFiles(@"D:\Work\FunctionalCSharp\Recursion");
            filesUnderDir.ForEach(Console.WriteLine);

            // Mutual Recursion
            var maleSequence = MaleSequence(55);
            var femaleSequence = FemaleSequence(25);
            Console.WriteLine("Male Sequence: {0}", maleSequence);
            Console.WriteLine("Female Sequence: {0}", femaleSequence);
        }
    }
}
