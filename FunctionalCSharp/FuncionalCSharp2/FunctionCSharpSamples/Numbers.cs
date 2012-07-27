using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionCSharpSamples
{
    public static class Numbers
    {
        public static void DisplayNonFuncional( )
        {             
            for ( int i = 0; i < 10; i++ )
                Console.WriteLine( i );
        }

        public static void DisplayFuncionalCSharp2( )
        {
            FunctionalExtensions.ForEach( FunctionCSharpSamples.Enumerable.Range( 0, 10 ), Display );
        }

        //public static void DisplayFuncionalCSharp3( )
        //{
        //    System.Linq.Enumerable.Range( 0, 10 ).ForEach( delegate( int i ) { Console.WriteLine( i ); } );
        //}

        //public static void DisplayFuncionalCSharp3Lambda( )
        //{
        //    System.Linq.Enumerable.Range( 0, 10 ).ForEach( i => Console.WriteLine( i ) );
        //}

        private static void Display( int i )
        {
            Console.WriteLine( i );
        }
    }
}