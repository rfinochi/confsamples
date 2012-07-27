using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ParallelFor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Opcion secuencial...");

			for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Inicio - " + i.ToString());
                Thread.Sleep(2000);
                Console.WriteLine("Fin - " + i.ToString());
			}

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("Opcion paralela...");
            
			#region Paralelo 
			
			Parallel.For(0, 10, i =>
			{
				Console.WriteLine("Inicio - " + i.ToString());
				Thread.Sleep(2000);
				Console.WriteLine("Fin - " + i.ToString());
			});
			
			#endregion
            
            Console.WriteLine("Fin total");

            Console.ReadKey();
        }
    }
}
