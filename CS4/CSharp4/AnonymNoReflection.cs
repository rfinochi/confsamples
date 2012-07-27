using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ConsoleApplication1
{
    public class AnonymNoReflection
    {
        public static void Main4( )
        {
            #region Look mom! No reflection!

            dynamic xx = 1;

            xx.Pepe();

            dynamic grupos = ObtenerObjetoAnonimo( );

            foreach ( var grupo in grupos )
                Console.WriteLine( grupo.Autor + " - " + grupo.Titulos.Length.ToString( ) );

            Console.ReadKey( );

            #endregion
        }

        public static dynamic ObtenerObjetoAnonimo( )
        {
            return
                from elements in XDocument.Load( new StreamReader( "Books.xml" ) ).Root.Elements( )
                group elements by elements.Attribute( "author" ).Value into grupoAutor
                select new
                {
                    Autor = grupoAutor.First( ).Attribute( "author" ).Value,
                    Titulos = ( from libro in grupoAutor
                                select libro.Attribute( "title" ) ).ToArray( )
                };
        }
    }
}