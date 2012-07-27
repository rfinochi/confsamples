using System;
using System.Linq;
using System.Threading;

namespace ParallelExtensions
{
    class Program
    {
        public static int[,] Multiply(int[,] a, int[,] b)
        {
            var n = a.GetLength(0);
            var c = new int[n, n];

            Action<int> rowTask = i =>
            {
                for(var j = 0; j < n; j++)
                    for(var k = 0; k < n; k++)
                        c[i, j] += a[i, k] * b[k, j];
            };

            for(var i = 0; i < n; i++)
                rowTask(i);

            return c;
        }

        public static int[,] ParallelMultiply(int[,] a, int[,] b)
        {
            var n = a.GetLength(0);
            var c = new int[n, n];

            Action<int> rowTask = i =>
            {
                for (var j = 0; j < n; j++)
                    for (var k = 0; k < n; k++)
                        c[i, j] = c[i, j] + a[i, k] * b[k, j];
            };

            Parallel.For(0, n, rowTask);

            return c;
        }

        private static void PrintResult(int[,] result)
        {
            for (var i = 0; i < result.GetUpperBound(0); i++)
            {
                for (var j = 0; j < result.GetUpperBound(1); j++)
                    Console.Write(result[i, j]);

                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            int[,] matrix1 = { { 1, 2, 5 }, { 2, 4, 1 }, { 3, 1, 2 } };
            int[,] matrix2 = { { 5, 1, 3 }, { 1, 3, 4 }, { 2, 4, 1 } };

            var serialResult = Multiply(matrix1, matrix2);
            PrintResult(serialResult);

            var parallelResult = ParallelMultiply(matrix1, matrix2);
            PrintResult(parallelResult);

            var items = ParallelEnumerable.Range(1, 100);
            var pResult = (from i in items
                          where i % 2 == 0
                          orderby i descending
                          select i * 2).AsParallel();
            foreach (var p in pResult) Console.WriteLine(p);
        }


    }
}
