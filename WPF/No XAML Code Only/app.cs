using System;
using System.Windows;

namespace CodeOnlyWindowsApplicationSample
{
    public class App : Application
    {
        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );

            MainWindow window = new MainWindow( );
            window.Show( );
        }
    }
}