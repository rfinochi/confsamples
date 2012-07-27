using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using IronPython.Runtime;

namespace Intro
{
    class Program
    {
        static void Main( string[] args )
        {
            #region Conozco la defincion del tipo

            //Calculator calc = GetCalculator( );
            //int sum = calc.Add( 10, 20 );

            #endregion

            #region No conozco el tipo!, pero tengo Reflection

            //object calc = GetCalculator( );
            //Type calcType = calc.GetType( );
            //object res = calcType.InvokeMember( "Add", BindingFlags.InvokeMethod, null, calc, new object[] { 10, 20 } );

            //int sum = Convert.ToInt32( res );

            #endregion

            #region El tipo esta en Python pero tengo

            //var engine = Python.CreateEngine(AppDomain.CurrentDomain);

            //var scope = engine.CreateScope();
            //engine.CreateScriptSourceFromFile(Path.Combine(Directory.GetCurrentDirectory(), "Calculator.py"), Encoding.Unicode, Microsoft.Scripting.SourceCodeKind.Statements).Execute(scope);

            //var calc = scope.GetVariable<IronPython.Runtime.Types.OldClass>("Calculator");

            //IronPython.Runtime.Types.OldInstance instance = (IronPython.Runtime.Types.OldInstance)calc.Call(null);
            //Method method = (Method)instance.__getattribute__(null, "Add");

            //int sum = (int)method.Call(null, 10, 20);

            #endregion

            #region Con dynamic la vida me sonrie

            //dynamic calc = GetCalculator( );
            //int sum = calc.Add( 10, 20 );

            #endregion

            #region El tipo esta en Python pero tengo .NET 4, Upi!!!

            var pythonEngine = IronPython.Hosting.Python.CreateRuntime( );
            dynamic pythonSample = pythonEngine.UseFile( Path.Combine( Directory.GetCurrentDirectory( ), "Calculator.py" ) );
            dynamic calc = pythonSample.Calculator( );

            int sum = calc.Sub( 10, 20 );

            #endregion

            Console.WriteLine( sum );
            Console.ReadLine( );
        }

        public static Calculator GetCalculator( )
        {
            return new Calculator( );
        }
    }
}