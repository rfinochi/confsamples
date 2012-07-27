using System;
using System.Collections.Generic;

namespace ConsoleApplication1
{
    public class CoContraVarianza
    {
        public static void Main2( )
        {
            var array = new string[] { "a", "b", "c" };
            Problem( array );
            CoVarianzaSafe( array );
            OtroMetodoCovariante( new Bar<string>( ) );
            OtroMetodoContravariante( new Foo<object>( ) );
        }

        // Co-varianza Unsafe
        public static void Problem( object[] arr )
        {
            //arr[0] = 4;
        }

        public static void CoVarianzaSafe( IEnumerable<object> elements )
        {
        }

        public static void OtroMetodoCovariante( IUnaInterfazCovariante<object> elements )
        {
            object unObj = elements.CreateNew( );

        }

        public static void OtroMetodoContravariante( IUnaInterfazContraVariante<string> elements )
        {
            elements.DoSomething( "a", "b" );
        }
    }

    public class Bar<T> : IUnaInterfazCovariante<T>
    {
        public void DoSomething( )
        {
            throw new NotImplementedException( );
        }

        public T CreateNew( )
        {
            throw new NotImplementedException( );
        }
    }

    public class Foo<T> : IUnaInterfazContraVariante<T>
    {
        public void DoSomething( T obj1, T obj2 )
        {
            throw new NotImplementedException( );
        }

        public object CreateNew( )
        {
            throw new NotImplementedException( );
        }
    }

    #region Interfaz con tipo Covariante

    public interface IUnaInterfazCovariante<out T>
    {
        void DoSomething( );
        T CreateNew( );
    }

    #endregion

    #region Interfaz con tipo Contra-Variante

    public interface IUnaInterfazContraVariante<in T>
    {
        void DoSomething( T obj1, T obj2 );
        object CreateNew( );
    }

    #endregion
}