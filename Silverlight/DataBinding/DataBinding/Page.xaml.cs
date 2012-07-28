using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DataBinding_Address
{
    public partial class Page : UserControl
    {
        Address address;

        public Page( )
        {
            InitializeComponent( );

            address = new Address( );

            txtName.DataContext = address;
            txtAddress1.DataContext = address;
            txtAddress2.DataContext = address;
            txtCity.DataContext = address;
            txtState.DataContext = address;
            txtZipcode.DataContext = address;
        }

        private void btnSave_Click( object sender, RoutedEventArgs e )
        {
            string newValues = "Name: " + address.Name + "\nAddress1 : " + address.Address1 + "\nAddress2 : " + address.Address2 + "\nCity: " + address.City + "\nState: " + address.State + "\nZipcode: " + address.Zipcode;
            MessageBox.Show( newValues );
        }

        private void btnClear_Click( object sender, RoutedEventArgs e )
        {
            address.Name = String.Empty;
            address.Address1 = String.Empty;
            address.Address2 = String.Empty;
            address.City = String.Empty;
            address.State = String.Empty;
            address.Zipcode = String.Empty;
        }
    }
}