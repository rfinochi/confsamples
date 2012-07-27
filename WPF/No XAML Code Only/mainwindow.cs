using System;
using System.Windows;
using System.Windows.Controls;

namespace CodeOnlyWindowsApplicationSample
{
    public class MainWindow : Window
    {
        protected override void OnInitialized( EventArgs e )
        {
            base.OnInitialized( e );

            Button closeButton = new Button( );
            closeButton.Content = "Close";
            this.Content = closeButton;

            closeButton.Click += delegate
            {
                this.Close( );
            };
        }
    }
}