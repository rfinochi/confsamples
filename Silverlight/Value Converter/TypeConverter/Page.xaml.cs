using System;
using System.Net;
using System.ServiceModel.Syndication;
using System.Windows;
using System.Windows.Controls;
using System.Xml;

namespace TypeConverter_CS
{
    public partial class Page : UserControl
    {
        public Page( )
        {
            InitializeComponent( );
            Loaded += new RoutedEventHandler( GetFeed );
        }

        void GetFeed( object sender, RoutedEventArgs e )
        {
            WebClient c = new WebClient( );
            c.OpenReadCompleted += new OpenReadCompletedEventHandler( ProcessFeed );
            c.OpenReadAsync( new Uri( "http://feeds.feedburner.com/shockbyte" ) );
        }

        void ProcessFeed( object sender, OpenReadCompletedEventArgs e )
        {
            XmlReader rdr = XmlReader.Create( e.Result );
            SyndicationFeed feed = SyndicationFeed.Load( rdr );

            FeedList.ItemsSource = feed.Items;
        }
    }
}