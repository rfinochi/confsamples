using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace CodeContracts
{
    class Program
    {
        static void Main( string[] args )
        {
            Contract.ContractFailed += new EventHandler<ContractFailedEventArgs>( Contract_ContractFailed );

            #region Snippet 4 - Programa con contratos

            Console.Write( "Ingrese la fecha de la factura: " );
            DateTime fechaFactura = DateTime.Parse( Console.ReadLine( ) );

            Console.Write( "Ingrese la fecha de entrega: " );
            DateTime fechaEntega = DateTime.Parse( Console.ReadLine( ) );

            Console.Write( "Ingrese el monto: " );
            string nro = Console.ReadLine( );
            Contract.Assume( nro != null ); // Int32.Parse tiene los Require incluidos!
            int monto = Int32.Parse( nro );
            Contract.Assume( monto == 37 );
            if ( fechaEntega >= fechaFactura && monto > 0 )
            {
                Factura f = new Factura( fechaEntega, fechaFactura, monto );
                Console.WriteLine( "Producto facturado!" );
            }
            else
            {
                Console.WriteLine( "Valores incorrectos!" );
            }

            Console.ReadKey( );

            #endregion
        }

        static void Contract_ContractFailed( object sender, ContractFailedEventArgs e )
        {
            Console.WriteLine( "No se pudo asumir algo: " + e.Condition );
            e.SetHandled( );
            Console.ReadKey( );
        }
    }

    #region Snippet 0 - Clase Factura

    public class Factura
    {
        //Propiedades
        private DateTime _fechaEntrega;
        public DateTime FechaEntrega
        {
            get
            {
                return _fechaEntrega;
            }
        }

        private DateTime _fechaFactura;
        public DateTime FechaFactura
        {
            get
            {
                return _fechaFactura;
            }
        }

        #region Snippet 2 - Propiedades

        int _monto;
        public int Monto
        {
            get
            {
                return _monto;
            }
            set
            {
                Contract.Requires( value >= 0 );
                _monto = value;
            }
        }

        #endregion

        //Constructor
        #region Snippet 2 - Constructor

        public Factura( DateTime fechaEntrega, DateTime fechaFactura, int monto )
        {
            Contract.Requires( fechaEntrega >= fechaFactura );
            Contract.Requires( monto >= 0 );

            _fechaEntrega = fechaEntrega;
            _fechaFactura = fechaFactura;
            _monto = monto;
        }

        #endregion

        //Metodos publicos
        #region Snippet 2 - Aplicar descuento

        public void AplicarDescuento( int porcentaje )
        {
            Contract.Requires( porcentaje >= 0 );
            Contract.Requires( porcentaje <= 100 );
            Contract.Ensures( Contract.OldValue( _monto ) >= _monto );

            int descuento = ( porcentaje * _monto ) / 100;
            Contract.Assume( descuento <= _monto );

            _monto = _monto - descuento;
        }

        #endregion

        #region Snippet 1 - Invariante de tipo

        [ContractInvariantMethod]
        void ObjectInvariant( )
        {
            Contract.Invariant( _fechaEntrega >= _fechaFactura );
            Contract.Invariant( _monto >= 0 );
        }

        #endregion
    }

    #endregion
}