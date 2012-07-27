using System;

namespace FunctionCSharpSamples
{
    class Program
    {
        static void Main( string[] args )
        {
            Numbers.DisplayNonFuncional( );
            Numbers.DisplayFuncionalCSharp2( );
            //Numbers.DisplayFuncionalCSharp3( );
            //Numbers.DisplayFuncionalCSharp3Lambda( );

            FileRead.NonFunctional( args[ 0 ] );
            //FileRead.Functional( args[ 0 ] );

            Console.ReadLine( );
        }
    }
}
