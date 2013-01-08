using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow( )
        {
            InitializeComponent( );
        }

        private void btnCalculate_Click( object sender, RoutedEventArgs e )
        {
            int number1 = int.Parse( txtNumber1.Text );
            int number2 = int.Parse( txtNumber1.Text );
            int result = Calculate( number1, number2 );

            txtAnswer.Text = result.ToString( );
            UploadResult( result );
        }

        private int Calculate( int number1, int number2 )
        {
            Thread.Sleep( 10000 );
            return number1 + number2;
        }

        private void UploadResult( int result )
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer( typeof( int ) );
            Stream JSonStream = new MemoryStream( );
            ser.WriteObject( JSonStream, result );
            JSonStream.Seek( 0, SeekOrigin.Begin );
            HttpClient client = new HttpClient( );
            HttpContent content = new StreamContent( JSonStream );
            client.PostAsync( "http://localhost/LogResultService", content );
        }

        //private async void btnCalculate_Click( object sender, RoutedEventArgs e )
        //{
        //    int number1 = int.Parse( txtNumber1.Text );
        //    int number2 = int.Parse( txtNumber1.Text );
        //    int result = await Calculate( number1, number2 );
        //    txtAnswer.Text = result.ToString( );
        //    await UploadResult( result );
        //}

        //private async Task<int> Calculate( int number1, int number2 )
        //{
        //    return await Task.Run( ( ) =>
        //    {
        //        Thread.Sleep( 10000 );
        //        return number1 + number2;
        //    } );
        //}

        //private async Task UploadResult( int result )
        //{
        //    DataContractJsonSerializer ser = new DataContractJsonSerializer( typeof( int ) );
        //    Stream JSonStream = new MemoryStream( );
        //    ser.WriteObject( JSonStream, result );
        //    JSonStream.Seek( 0, SeekOrigin.Begin );
        //    HttpClient client = new HttpClient( );
        //    HttpContent content = new StreamContent( JSonStream );
        //    await client.PostAsync( "http://localhost/LogResultService", content );
        //}
    }
}