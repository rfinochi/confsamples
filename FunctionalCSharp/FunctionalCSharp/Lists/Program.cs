using System;
using System.Collections.Generic;
using System.Linq;

namespace Lists
{
    static class ListExtensions
    {
        // F# Seq.iter
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var item in items)
                action(item);
        }

        public static int Length<T>(this IImmutableList<T> list)
        {
            if (list == null)
                return 0;

            return 1 + Length(list.Tail);
        }

        public static int Sum(this IImmutableList<int> list)
        {
            if (list.IsEmpty)
                return 0;

            return list.Head + Sum(list.Tail);
        }

        public static T Last<T>(this IImmutableList<T> items)
        {
            if (items.IsEmpty)
                throw new ArgumentNullException("items");

            var tail = items.Tail;
            return tail.IsEmpty ? items.Head : tail.Last();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Calculate list length
            var enumerator = Enumerable.Range(1, 10).GetEnumerator();
            var list = LazyList<int>.Create(enumerator);
            var length = list.Length();
            var sum = list.Sum();
            var last = list.Last();
            Console.WriteLine("List length: {0}", length);
            Console.WriteLine("List sum: {0}", sum);
            Console.WriteLine("Last value: {0}", last);

            // Add to queue
            var queue = ImmutableQueue<int>.Empty;
            queue = queue.Enqueue(3);
            queue = queue.Enqueue(5);
            queue.ForEach(Console.WriteLine);

            // Add to stack
            var stack = ImmutableStack<int>.Empty;
            stack = stack.Push(3);
            stack = stack.Push(5);
            stack.ForEach(Console.WriteLine);
        }
    }
}
