namespace CalculaPI
{
    using System;
    using System.Threading.Tasks;

    class Program
    {
        public static void CalculaPiSecuencial(int dartsPerProcessor, int processorsSize)
        {            
            int totalDartsInCircle = 0;

            var startTime = DateTime.Now;

            for (int j = 0; j < processorsSize; j++)
            {
                Random random = new Random(5 * j);
                int dartsInCircle = 0;
                for (int i = 0; i < dartsPerProcessor; ++i)
                {
                    double x = (random.NextDouble() - 0.5) * 2;
                    double y = (random.NextDouble() - 0.5) * 2;

                    if ((x * x) + (y * y) <= 1.0)
                    {
                        ++dartsInCircle;
                    }
                }

                totalDartsInCircle += dartsInCircle;
            }

            var endTime = DateTime.Now;

            Console.WriteLine("Pi is approximately {0:F15}.", 4 * (double)totalDartsInCircle / (processorsSize * (double)dartsPerProcessor));
            Console.WriteLine("Total Time: {0}  milliseconds.", (endTime - startTime).TotalMilliseconds);

            Console.ReadKey();
        }

        private static object controlConcurrency = new object();

        static void CalculaPiParallel(int dartsPerProcessor, int processorsSize)
        {            
            int totalDartsInCircle = 0;

            var startTime = DateTime.Now;

            Parallel.For(
                0,
                processorsSize,
                delegate(int j)
                {
                    Random random = new Random(5 * j);
                    int dartsInCircle = 0;
                    for (int i = 0; i < dartsPerProcessor; ++i)
                    {
                        double x = (random.NextDouble() - 0.5) * 2;
                        double y = (random.NextDouble() - 0.5) * 2;
                        if ((x * x) + (y * y) <= 1.0)
                        {
                            ++dartsInCircle;
                        }
                    }

                    lock (controlConcurrency)
                    {
                        totalDartsInCircle += dartsInCircle;
                    }
                });

            var endTime = DateTime.Now;

            Console.WriteLine("Pi is approximately {0:F15}.", 4 * (double)totalDartsInCircle / (processorsSize * (double)dartsPerProcessor));
            Console.WriteLine("Total Time: {0}  milliseconds.", (endTime - startTime).TotalMilliseconds);

            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            CalculaPiSecuencial(10000, 100000);
            CalculaPiParallel(10000, 100000);
        }
    }
}
