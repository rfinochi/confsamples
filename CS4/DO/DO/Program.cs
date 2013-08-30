using System;
using System.Linq;
using System.Xml.Linq;

class Program
{
    static void Main( string[] args )
    {
        var custs = from c in XElement.Load( @"..\..\Customers.xml" ).Elements( )
                    select new DynamicElement( c );


        foreach ( dynamic d in custs )
        {
            Console.WriteLine( String.Format( "Name: {0} {1}", d.Id, d.LastName ) );

            d.ZipCode = "10001";
            Console.WriteLine( String.Format( "ZipCode: {0}", d.ZipCode ) );

            Console.WriteLine( d.ToXml( ) );
        }

        Console.ReadLine( );
    }
}