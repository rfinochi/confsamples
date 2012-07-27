using DemoApp.DataAccess;
using DemoApp.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class CustomerRepositoryTests
	{
		[TestMethod]
		public void TestAllCustomersAreLoaded( )
		{
			CustomerRepository target = new CustomerRepository( Constants.CUSTOMER_DATA_FILE );
			Assert.AreEqual( 3, target.GetCustomers( ).Count, "Invalid number of customers in repository." );
		}

		[TestMethod]
		public void TestNewCustomerIsAddedProperly( )
		{
			CustomerRepository target = new CustomerRepository( Constants.CUSTOMER_DATA_FILE );
			Customer cust = Customer.CreateNewCustomer( );

			bool eventArgIsValid = false;
			target.CustomerAdded += ( sender, e ) => eventArgIsValid = ( e.NewCustomer == cust );
			target.AddCustomer( cust );

			Assert.IsTrue( eventArgIsValid, "Invalid NewCustomer property" );
			Assert.IsTrue( target.ContainsCustomer( cust ), "New customer was not added" );
		}
	}
}