namespace CalculoPI
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        public static object controlConcurrency = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Version Secuencial...");

            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Inicio - " + i.ToString());
                Thread.Sleep(2000);
                Console.WriteLine("Fin - " + i.ToString());
            }

			/*
            Console.WriteLine("Version Parallel...");
            Console.ReadLine();

            Parallel.For(0, 10, i =>
                {
                    Console.WriteLine("Inicio - " + i.ToString());
                    Thread.Sleep(2000);
                    Console.WriteLine("Fin - " + i.ToString());
                });

            Console.WriteLine("Fin total");
			*/
            Console.ReadKey();
        }
    }
}
