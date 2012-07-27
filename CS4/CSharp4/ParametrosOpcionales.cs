using System;

namespace ConsoleApplication1
{
    class ParametrosOpcionales
    {
        public static void Main2( )
        {
            MetodoConOpcionales(
                p3: 3.5f,
                p1: 1,
                p2: "Hola" );
        }

        #region Parametros comunes

        public static void MetodoComun( int p1 )
        {
            MetodoComun( p1, "lalaa", 2.5f );
        }

        public static void MetodoComun( int p1, string p2 )
        {
            MetodoComun( p1, p2, 2.5f );
        }

        public static void MetodoComun( int p1, float p3 )
        {
            MetodoComun( p1, "lalaa", p3 );
        }

        public static void MetodoComun( int p1, string p2, float p3 )
        {
            Console.WriteLine( p1 );
            Console.WriteLine( p2 );
            Console.WriteLine( p3 );
        }

        #endregion

        #region Parametros opcionales

        public static void MetodoConOpcionales( int p1, string p2 = "lalaa", float p3 = 2.5f )
        {
            Console.WriteLine( p1 );
            Console.WriteLine( p2 );
            Console.WriteLine( p3 );
        }

        #endregion
    }
}