using System;
using System.Collections.Generic;
using System.Linq;

namespace Currying
{
    public static class FunctionExtensions
    {
        // F# - Currying
        public static Func<TArg1, Func<TArg2, TResult>> Curry<TArg1, TArg2, TResult>(this Func<TArg1, TArg2, TResult> func)
        {
            return a1 => a2 => func(a1, a2);
        }

        // F# - Currying
        public static Func<TArg1, Action<TArg2>> Curry<TArg1, TArg2>(this Action<TArg1, TArg2> action)
        {
            return a1 => a2 => action(a1, a2);
        }

        // F# Seq.iter
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Currying
            // F#
            // let multiply x y = x * y
            // let multiplyByThree = multiply 3
            // let multiplyResult = multiplyByThree 15
            Func<int, int, int> multiply = (x, y) => x * y;

            var curriedMultiply = multiply.Curry();
            var curriedMultiplyThree = curriedMultiply(3);
            var curriedMultiplyResult = curriedMultiplyThree(15);
            Console.WriteLine("Result of 3 * 15 = {0}", curriedMultiplyResult);

            // Currying action
            // F#
            // let contains l i = printfn "Item %i is contained: %b" item (l |> List.exists(fun x -> x = i))
            // let containsRange = contains [1..10]
            // let containsResult = containsRange 3
            Action<IEnumerable<int>, int> contains = (items, item) => Console.WriteLine("Item {0} is contained: {1}", item, items.Contains(item));

            var curriedContains = contains.Curry();
            var curriedContainsRange = curriedContains(Enumerable.Range(1, 10));
            curriedContainsRange(3);
        }
    }
}
