using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;

using DemoApp.Model;

namespace DemoApp.DataAccess
{
	public class CustomerRepository
	{
		#region Fields

		private readonly List<Customer> _customers;

		private readonly string _customerDataFile;

		#endregion

		#region Constructors

		public CustomerRepository( string customerDataFile )
		{
			_customerDataFile = customerDataFile;
			_customers = LoadCustomers( );
		}

		#endregion

		#region Public Interface

		public event EventHandler<CustomerAddedEventArgs> CustomerAdded;

		public void AddCustomer( Customer customer )
		{
			if ( customer == null )
				throw new ArgumentNullException( "customer" );

			if ( !_customers.Contains( customer ) )
			{
				_customers.Add( customer );

				if ( this.CustomerAdded != null )
					this.CustomerAdded( this, new CustomerAddedEventArgs( customer ) );
			}
		}

		public void Save( )
		{
			XmlDocument doc = new XmlDocument( );
			XmlNode customersNode = doc.AppendChild( doc.CreateElement( "customers" ) );

			foreach ( Customer customer in _customers )
			{
				XmlNode customerNode = customersNode.AppendChild( doc.CreateElement( "customer" ) );
				customerNode.Attributes.Append( doc.CreateAttribute( "lastName" ) ).Value = customer.LastName;
				customerNode.Attributes.Append( doc.CreateAttribute( "firstName" ) ).Value = customer.FirstName;
				customerNode.Attributes.Append( doc.CreateAttribute( "isCompany" ) ).Value = customer.IsCompany.ToString( );
				customerNode.Attributes.Append( doc.CreateAttribute( "email" ) ).Value = customer.Email;
				customerNode.Attributes.Append( doc.CreateAttribute( "totalSales" ) ).Value = customer.TotalSales.ToString( );
			}

			doc.Save( _customerDataFile );
		}

		public bool ContainsCustomer( Customer customer )
		{
			if ( customer == null )
				throw new ArgumentNullException( "customer" );

			return _customers.Contains( customer );
		}

		public List<Customer> GetCustomers( )
		{
			return new List<Customer>( _customers );
		}

		public Customer GetCustomer( string customerFirstName, string customerLastName )
		{
			if ( String.IsNullOrEmpty( customerLastName ) )
				return ( from c in _customers
						 where c.FirstName == customerFirstName
						 select c ).FirstOrDefault( );
			else
				return ( from c in _customers
						 where c.FirstName == customerFirstName && c.LastName == customerLastName
						 select c ).FirstOrDefault( );
		}

		#endregion

		#region Private Helpers

		private List<Customer> LoadCustomers( )
		{
			using ( Stream stream = File.OpenRead( _customerDataFile ) )
			using ( XmlReader xmlRdr = new XmlTextReader( stream ) )
				return
					( from customerElem in XDocument.Load( xmlRdr ).Element( "customers" ).Elements( "customer" )
					  select Customer.CreateCustomer(
						 Convert.ToDouble( customerElem.Attribute( "totalSales" ).Value, CultureInfo.CurrentCulture ),
						 (string)customerElem.Attribute( "firstName" ),
						 (string)customerElem.Attribute( "lastName" ),
						 (bool)customerElem.Attribute( "isCompany" ),
						 (string)customerElem.Attribute( "email" )
						  ) ).ToList( );
		}

		private static Stream GetResourceStream( string resourceFile )
		{
			Uri uri = new Uri( resourceFile, UriKind.RelativeOrAbsolute );

			StreamResourceInfo info = Application.GetResourceStream( uri );
			if ( info == null || info.Stream == null )
				throw new ApplicationException( "Missing resource file: " + resourceFile );

			return info.Stream;
		}

		#endregion
	}
}