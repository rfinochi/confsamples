using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace Unfolding
{
    static class SequenceExtensions
    {
        // F# Seq.unfold
        public static IEnumerable<TResult> Unfold<T, TResult>(Func<T, Option<Tuple<TResult, T>>> generator, T start)
        {
            var next = start;

            while (true)
            {
                var res = generator(next);
                if (res.IsNone)
                    yield break;

                yield return res.Value.Item1;

                next = res.Value.Item2;
            }
        }

        // Seq.init_infinite
        public static IEnumerable<T> InitializeInfinite<T>(Func<int, T> f)
        {
            return Unfold(s => Option.Some(Tuple.New(f(s), s + 1)), 0);
        }

        // Seq.init_finite
        public static IEnumerable<T> InitializeFinite<T>(int count, Func<int, T> f)
        {
            return Unfold(s => s < count ? Option.Some(Tuple.New(f(s), s + 1)) : Option<Tuple<T, int>>.None, 0);
        }

        // F# Seq.iter
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        // F# Seq.generate
        public static IEnumerable<TResult> Generate<T, TResult>(Func<T> opener, Func<T, Option<TResult>> generator, Action<T> closer)
        {
            var openerResult = opener();

            while (true)
            {
                var res = generator(openerResult);
                if (res.IsNone)
                {
                    closer(openerResult);
                    yield break;
                }

                yield return res.Value;
            }
        }

        public static IEnumerable<TResult> GenerateUsing<T, TResult>(Func<T> opener, Func<T, Option<TResult>> generator) where T : IDisposable
        {
            return Generate(opener, generator, x => x.Dispose());
        }
    }

    class Program
    {
        public static IEnumerable<long> Fibonacci
        {
            get
            {
                var i = 1;
                var j = 1;
                while (true)
                {
                    yield return i;
                    var temp = i;
                    i = j;
                    j = j + temp;
                }
            }
        }

        static void Main(string[] args)
        {
            // Imperative Fibonacci
            var imperativeFibs = Fibonacci.Take(20);
            imperativeFibs.ForEach(Console.WriteLine);

            // F# - let fibs = Seq.unfold (fun (i,j) -> Some(i,(j,i+j))) (1,1)
            var fibs = SequenceExtensions.Unfold(x => Option.Some(Tuple.New(x.Item1, Tuple.New(x.Item2, x.Item1 + x.Item2))), Tuple.New(1, 1));
            fibs.Take(20).ForEach(Console.WriteLine);

            // F# - let allCubes = Seq.init_infinite (fun x -> x * x * x)
            var allCubes = SequenceExtensions.InitializeInfinite(x => x * x * x);
            allCubes.Take(20).ForEach(Console.WriteLine);

            // F# - let tenCubes = Seq.init_finite 10 (fun x -> x * x * x)
            var first20Cubes = SequenceExtensions.InitializeFinite(20, x => x * x * x);
            first20Cubes.ForEach(Console.WriteLine);

            // F# - let htmlList = Seq.generate (fun () ->
            //        File.OpenText(@"D:\Tools\Reflector\ReadMe.htm")) (fun x ->
            //          if x.EndOfStream then None else Some(x.ReadLine())) (fun x ->
            //            x.Close())
            var htmlList = SequenceExtensions.Generate(() => File.OpenText(@"D:\Tools\Reflector\ReadMe.htm"), x => x.EndOfStream ? Option<string>.None : Option.Some(x.ReadLine()), x => x.Dispose());
            htmlList.ForEach(Console.WriteLine);

            // F# - let htmlListUsing = Seq.generate_using (fun () ->
            //        File.OpenText(@"D:\Tools\Reflector\ReadMe.htm")) (fun x ->
            //          if x.EndOfStream then None else Some(x.ReadLine()))
            var htmlListUsing = SequenceExtensions.GenerateUsing(() => File.OpenText(@"D:\Tools\Reflector\ReadMe.htm"), x => x.EndOfStream ? Option<string>.None : Option.Some(x.ReadLine()));
            htmlListUsing.ForEach(Console.WriteLine);
        }
    }
}