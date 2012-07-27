using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

using DemoApp.ViewModel;

namespace DemoApp
{
	public partial class App : Application
	{
		#region Constants

		public const string ViewAllCustomerArg = "/viewAllCustomer";

		#endregion

		#region Fields

		public static readonly string ExecutablePath = Assembly.GetEntryAssembly( ).Location;
		public static readonly string ExecutableFolder = Path.GetDirectoryName( ExecutablePath );

		#endregion

		static App( )
		{
			// This code is used to test the app when using other cultures.
			//
			//System.Threading.Thread.CurrentThread.CurrentCulture =
			//    System.Threading.Thread.CurrentThread.CurrentUICulture =
			//        new System.Globalization.CultureInfo("it-IT");


			// Ensure the current culture passed into bindings is the OS culture.
			// By default, WPF uses en-US as the culture, regardless of the system settings.
			//
			FrameworkElement.LanguageProperty.OverrideMetadata(
			  typeof( FrameworkElement ),
			  new FrameworkPropertyMetadata( XmlLanguage.GetLanguage( CultureInfo.CurrentCulture.IetfLanguageTag ) ) );
		}

		protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

			MainWindow window = new MainWindow( );

			string path = @"C:\Trabajos\Conferencias\RUN 09 - Arquitecturas de presentacion con WPF\Samples\MvvmDemoApp_Spanish\DemoApp\bin\Debug\customers.xml";
			var viewModel = new MainWindowViewModel( path );

			EventHandler handler = null;
			handler = delegate
			{
				viewModel.RequestClose -= handler;
				window.Close( );
			};
			viewModel.RequestClose += handler;

			window.DataContext = viewModel;

			window.Show( );

			if ( e.Args != null && e.Args.GetUpperBound( 0 ) == 0 )
			{
				if ( e.Args[ 0 ] == ViewAllCustomerArg )
				{
					viewModel.ShowAllCustomers( );
				}
				else
				{
					string[] customerData = Path.GetFileNameWithoutExtension( e.Args[ 0 ].Replace( "/doc", String.Empty ) ).Split( '_' );

					if ( customerData.GetUpperBound( 0 ) == 0 )
						viewModel.ShowCustomer( customerData[ 0 ], null );
					else
						viewModel.ShowCustomer( customerData[ 0 ], customerData[ 1 ] );
				}
			}
		}
	}
}