using System;

namespace ConsoleApplication1
{
    public class Proceso
    {
        public override string ToString( )
        {
            return String.Format( "PID: {0} - Nombre: {1}", PID, Nombre );
        }

        public int PID
        {
            get;
            set;
        }

        public int Prioridad
        {
            get;
            set;
        }

        public string Nombre
        {
            get;
            set;
        }

        public int PorcentajeEjecucion
        {
            get;
            set;
        }
    }
}