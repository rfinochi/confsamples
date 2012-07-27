using System;
using System.IO;

namespace ConsoleApplication1
{
    class HelloPython
    {
        public static void Main2( )
        {
            #region Saludo desde Python

            var pythonEngine = IronPython.Hosting.Python.CreateRuntime( );
            dynamic pythonSample = pythonEngine.UseFile( Path.Combine( Directory.GetCurrentDirectory( ), "PythonSample.py" ) );
            dynamic claseDePython = pythonSample.ClaseDePython( );

            Console.WriteLine( "ObtenerSaludo: " + claseDePython.ObtenerSaludo( ) );
            Console.WriteLine( "prop1: " + claseDePython.prop1 );
            Console.WriteLine( "prop2: " + claseDePython.prop2 );

            Console.ReadKey( );

            #endregion
        }
    }
}