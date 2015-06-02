using System;
using System.Windows.Forms;

namespace SignalRChat
{
    public static class Program
    {
        internal static WinFormsServer MainForm { get; set; }
 
        [STAThread]
        static void Main( )
        {
            Application.EnableVisualStyles( );
            Application.SetCompatibleTextRenderingDefault( false );
            MainForm = new WinFormsServer( );
            Application.Run( MainForm );
        }
    }
}