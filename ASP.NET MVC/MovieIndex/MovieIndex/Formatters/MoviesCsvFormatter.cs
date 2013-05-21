using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

using MovieIndex.Models;

namespace MovieIndex.Formatters
{
    public class MoviesCsvFormatter : BufferedMediaTypeFormatter
    {
        private static char[] _specialChars = new char[] { ',', '\n', '\r', '"' };

        public MoviesCsvFormatter( )
        {
            SupportedMediaTypes.Add( new MediaTypeHeaderValue( "text/csv" ) );
        }

        public override bool CanWriteType( Type type )
        {
            if ( type == typeof( Movie ) )
            {
                return true;
            }
            else
            {
                Type enumerableType = typeof( IEnumerable<Movie> );
                return enumerableType.IsAssignableFrom( type );
            }
        }

        public override bool CanReadType( Type type )
        {
            return false;
        }

        public override void WriteToStream( Type type, object value, Stream writeStream, HttpContent content )
        {
            using ( var writer = new StreamWriter( writeStream ) )
            {

                var movies = value as IEnumerable<Movie>;
                if ( movies != null )
                {
                    foreach ( var product in movies )
                        WriteItem( product, writer );
                }
                else
                {
                    var singleMovie = value as Movie;
                    if ( singleMovie == null )
                        throw new InvalidOperationException( "Cannot serialize type" );

                    WriteItem( singleMovie, writer );
                }
            }
            writeStream.Close( );
        }

        private void WriteItem( Movie movie, StreamWriter writer )
        {
            writer.WriteLine( "{0},{1},{2},{3},{4},{5}", Escape( movie.Id ),
                                                            Escape( movie.Name ),
                                                            Escape( movie.DirectorName ),
                                                            Escape( movie.GenreId ),
                                                            Escape( movie.ReleaseDate ),
                                                            Escape( movie.Rating ) );
        }

        private string Escape( object o )
        {
            if ( o == null )
                return String.Empty;

            string field = o.ToString( );
            if ( field.IndexOfAny( _specialChars ) != -1 )
                return String.Format( "\"{0}\"", field.Replace( "\"", "\"\"" ) );
            else
                return field;
        }
    }
}