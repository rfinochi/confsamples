using System;
using System.IO;
using System.Net.Http;

namespace MovieIndex.WebApiTestClient
{
    class Program
    {
        static void Main( string[] args )
        {
            HttpClient client = new HttpClient( );

            //client.DefaultRequestHeaders.Accept.Add( new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue( "application/xml" ) );
            //client.DefaultRequestHeaders.Accept.Add( new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue( "text/csv" ) );
            client.DefaultRequestHeaders.Accept.Add( new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue( "text/html" ) );
            string result = client.GetStringAsync( "http://localhost:1123//api/movies/" ).Result;
            
            File.WriteAllText( "movies.csv", result );

            Console.ReadLine( );
        }
    }
}
