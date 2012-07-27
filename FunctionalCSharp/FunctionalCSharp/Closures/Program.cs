using System;
using System.Collections.Generic;
using System.Linq;

namespace Closures
{
    public static class ClosureExtensions
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
        static Func<int> AddThree(int n)
        {
            return () => n + 3;
        }

        static void Main(string[] args)
        {
            // Simple closure
            var value = 5;
            Func<int, int> multiply = x => (x * x) + value;
            var mulResult = multiply(3);
            Console.WriteLine("Multiply value: {0}", mulResult);

            // Parameterized
            var addThree = AddThree(4);
            var addThreeResult = addThree();
            Console.WriteLine("Add Three Result: {0}", addThreeResult);
        }
    }
}
