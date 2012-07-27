namespace ParallelControlProcess
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Control de concurrencia ...");

            Parallel.For(0, 10, delegate(int i, ParallelLoopState s)
            {
                if (i == 4)
                {
                    s.Stop();
                }

                Console.WriteLine("Inicio - " + i.ToString());
                Thread.Sleep(2000);

                if (s.IsStopped)
                {
                    Console.WriteLine("STOPED " + i);
                }

                Console.WriteLine("Fin - " + i.ToString());
            });

            Console.WriteLine("Fin total");
            Console.ReadKey();
        }
    }
}
