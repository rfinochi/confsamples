namespace SamplePLINQ
{
    using System;
    using System.Linq;

    class Program
    {
        public static void SamplePLINQ()
        {
            var elementos = Enumerable.Range(1, Int32.MaxValue);

            //var query = from e in elementos where e % 2 == 0 select e;

            var query = from e in elementos.AsParallel() where e % 20 == 0 select e;

			query.ToList().ForEach((i) => Console.WriteLine(i));
        }

        static void Main(string[] args)
        {
            SamplePLINQ();
            Console.ReadLine();
        }
    }
}
