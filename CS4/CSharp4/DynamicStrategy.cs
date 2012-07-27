using System;
using System.IO;

namespace ConsoleApplication1
{
    class DynamicStrategy
    {
        public static void Main( )
        {
            #region Interaccion con Python

            var procesos = new[]
			{
			    new Proceso()
			    {
			        Nombre = "BatchProcessXX.exe",
			        PID = 5618,
			        PorcentajeEjecucion = 66,
			        Prioridad = 10
			    },

			    new Proceso()
			    {
			        Nombre = "Antivirus.exe",
			        PID = 8472,
			        PorcentajeEjecucion = 12,
			        Prioridad = 2
			    },

			    new Proceso()
			    {
			        Nombre = "UpdatesDownloader.exe",
			        PID = 125,
			        PorcentajeEjecucion = 37,
			        Prioridad = 1
			    },

			    new Proceso()
			    {
			        Nombre = "Parser.exe",
			        PID = 3259,
			        PorcentajeEjecucion = 72,
			        Prioridad = 30
			    }
			};

            var pythonEngine = IronPython.Hosting.Python.CreateRuntime( AppDomain.CurrentDomain );

            //Cambiar la seleccion de archivo por un Console.ReadLine
            string strategyFileName = String.Empty;
            do
            {
                try
                {
                    Console.Write( "Estrategia: " );
                    strategyFileName = Console.ReadLine( );

                    dynamic strategy = pythonEngine.UseFile( Path.Combine( Directory.GetCurrentDirectory( ), strategyFileName ) );
                    Proceso proceso = strategy.ElegirProceso( procesos );

                    Console.WriteLine( "Proceso: " + proceso.ToString( ) );
                    Console.ReadKey( );
                }
                catch
                {
                }

            } while ( strategyFileName != "exit" );

            #endregion
        }
    }
}