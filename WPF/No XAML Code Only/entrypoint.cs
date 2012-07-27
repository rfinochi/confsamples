using System;

namespace CodeOnlyWindowsApplicationSample
{
    public class EntryPoint
    {
        [STAThread]
        public static void Main( )
        {
            App app = new App( );
            app.Run( );
        }
    }
}