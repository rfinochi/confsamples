using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FunctionCSharpSamples
{
    public static class FileRead
    {
        public static void NonFunctional( string filePath )
        {
            using ( StreamReader reader = File.OpenText( filePath ) )
            {
                String contents = reader.ReadToEnd( );
                String[] words = contents.Split( ' ' );

                for ( int i = 0; i < words.Length; i++ )
                    words[ i ] = words[ i ].Trim( );

                Dictionary<String, int> dictionary = new Dictionary<string, int>( );

                foreach ( String word in words )
                {
                    if ( dictionary.ContainsKey( word ) )
                        dictionary[ word ]++;
                    else
                        dictionary.Add( word, 1 );
                }

                foreach ( KeyValuePair<String, int> kvp in dictionary )
                    Console.WriteLine( String.Format( "{0} count: {1}", kvp.Key, kvp.Value.ToString( ) ) );
            }
        }

        //private static void Functional( string filePath )
        //{
        //    File.OpenText( filePath ).Use( stream =>
        //    {
        //        stream
        //            .ReadToEnd( )
        //            .Split( ' ' )
        //            .Convert( str => str.Trim( ) )
        //            .GetCounts( ( x, y ) => x == y )
        //            .ForEach( kvp => String.Format( "{0} count: {1}", kvp.Key, kvp.Value.ToString( ) ) );
        //    } );
        //}
    }
}
