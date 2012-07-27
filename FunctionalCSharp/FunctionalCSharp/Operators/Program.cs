using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Operators
{
    public static class FunctionExtensions
    {
        // F# - |>
        public static TResult Forward<TArg1, TArg2, TResult>(this TArg1 arg1, Func<TArg1, TArg2, TResult> func, TArg2 arg2)
        {
            return func(arg1, arg2);
        }

        // F# - |>
        public static void Forward<TArg1, TArg2>(this TArg1 arg1, Action<TArg1, TArg2> action, TArg2 arg2)
        {
            action(arg1, arg2);
        }

        // F# <|
        public static TResult Rev<TArg1, TArg2, TResult>(this TArg2 arg2, Func<TArg1, TArg2, TResult> func, TArg1 arg1)
        {
            return func(arg1, arg2);
        }

        // F# - <|
        public static void Rev<TArg1, TArg2>(this TArg2 arg2, Action<TArg1, TArg2> action, TArg1 arg1)
        {
            action(arg1, arg2);
        }

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
        public static bool Contains(IEnumerable<int> items, int item)
        {
            return items.Contains(item);
        }

        public static void IsContained(IEnumerable<int> items, int item)
        {
            Console.WriteLine("Item {0} contained: {1}", item, items.Contains(item));
        }

        static void Main(string[] args)
        {
            var range = Enumerable.Range(1, 10);

            // Forward with Func
            // F# - [1..10] |> List.exists (fun x -> x = 3)
            bool containsResult = range.Forward<IEnumerable<int>, int, bool>(Contains, 3);
            Console.WriteLine("Is contained: {0}", containsResult);

            // Forward with Action
            // F# - [1..10] |> (fun x -> printfn "Item %i contained: %b" x (List.exists(fun y -> y = 3)))
            range.Forward(IsContained, 3);

            // Reverse with Func
            bool containsResultRev = 3.Rev<IEnumerable<int>, int, bool>(Contains, range);
            Console.WriteLine("Is contained: {0}", containsResultRev);

            // Reverse with Action
            3.Rev(IsContained, range);
        }
    }
}
