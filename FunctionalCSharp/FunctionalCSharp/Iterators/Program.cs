using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Iterators
{
    static class SequenceExtensions
    {
        // F# Seq.iter
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if(items == null) throw new ArgumentNullException("items");
            if(action == null) throw new ArgumentNullException("action");

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

    }

    static class Program
    {
        [DllImport("msvcrt.dll")]
        unsafe private static extern byte* strcpy(char* dst, char* src);

        static void ImperativeLoop()
        {
            var items = Enumerable.Range(1, 10);
            foreach (var item in items)
                Console.WriteLine(item);
        }

        static void Main(string[] args)
        {
            // Imperative
            ImperativeLoop();

            // Array
            var array = Enumerable.Range(1, 10).ToArray();
            Array.ForEach(array, Console.WriteLine);

            // List<T>
            var list = Enumerable.Range(1, 10).ToList();
            list.ForEach(Console.WriteLine);

            // F# - [1..10] |> List.iter(fun x-> printfn "%i" i)
            Enumerable.Range(1, 10).ForEach(Console.WriteLine);

            // F# - [1..10] |> List.iteri(fun i x -> printfn "%i - %i" i x)
            Enumerable.Range(1, 10).ForIndex((i, x) => Console.WriteLine("{0} - {1}", i, x));
        }
    }
}
