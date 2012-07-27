using System;
using System.Collections.Generic;
using System.Linq;

namespace Fold
{
    static class SequenceExtensions
    {
        // Seq.fold
        public static T Fold<T, U>(this IEnumerable<U> items, Func<T, U, T> func, T acc)
        {
            foreach (var item in items)
                acc = func(acc, item);

            return acc;
        }

        // F# List.fold_left
        public static T FoldLeft<T, U>(this IList<U> list, Func<T, U, T> func, T acc)
        {
            for (var index = 0; index < list.Count; index++)
                acc = func(acc, list[index]);

            return acc;
        }

        // F# List.fold_right
        public static T FoldRight<T, U>(this IList<U> list, Func<T, U, T> func, T acc)
        {
            for (var index = list.Count - 1; index >= 0; index--)
                acc = func(acc, list[index]);

            return acc;
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
        static int Sum(IEnumerable<int> items)
        {
            var sum = 0;

            foreach (var item in items)
                sum += item;

            return sum;
        }

        static void Main(string[] args)
        {
            // Imperative Sum
            var sumImperative = Sum(Enumerable.Range(1, 10));
            Console.WriteLine("Imperative sum value: {0}", sumImperative);

            // F# - [1..10] |> Seq.fold(fun acc x -> acc + x) 1
            var fold = Enumerable.Range(1, 10).Fold((acc, x) => acc + x, 0);
            Console.WriteLine("Fold value: {0}", fold);

            // LINQ Aggregate
            var aggregate = Enumerable.Range(1, 10).Aggregate(0, (acc, x) => acc + x);
            Console.WriteLine("Aggregate value: {0}", aggregate);

            // LINQ Sum
            var sum = Enumerable.Range(1, 10).Sum();
            Console.WriteLine("Sum value: {0}", sum);

            // F# - [1..10] |> List.fold_left (fun acc x -> acc + x) 1
            var first10 = Enumerable.Range(1, 10).ToList();
            var foldLeft = first10.FoldLeft((acc, x) => acc + x, 0);
            Console.WriteLine("Fold left value: {0}", foldLeft);

            // F# - List.fold_right (fun acc x -> Math.Max(acc, x) [5;9;1;3;4;3;] int.MinValue
            var foldRight = new List<int> { 5, 9, 1, 3, 4, 3 }.FoldRight(Math.Max, int.MinValue);
            Console.WriteLine("Fold right value: {0}", foldRight);
        }
    }
}
