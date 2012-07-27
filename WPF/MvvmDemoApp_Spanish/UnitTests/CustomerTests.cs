using DemoApp.Model;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class CustomerTests
	{
		[TestMethod]
		public void TestIsValidPerson( )
		{
			Customer target = Customer.CreateNewCustomer( );
			target.IsCompany = false;
			target.FirstName = "Lloyd";
			target.LastName = "Christmas";
			target.Email = "lloyd@acme.com";

			Assert.IsTrue( target.IsValid, "Should be valid" );
		}

		[TestMethod]
		public void TestIsValidCompany( )
		{
			Customer target = Customer.CreateNewCustomer( );
			target.IsCompany = true;
			target.FirstName = "Acme, Inc.";
			target.Email = "email@acme.com";

			Assert.IsTrue( target.IsValid, "Should be valid" );
		}

		[TestMethod]
		public void TestIsInvalidPersonIfEmpty( )
		{
			Customer target = Customer.CreateNewCustomer( );

			Assert.IsFalse( target.IsValid, "Should be invalid" );
		}

		[TestMethod]
		public void TestIsInvalidCompanyIfLastNameExists( )
		{
			Customer target = Customer.CreateNewCustomer( );
			target.IsCompany = true;
			target.FirstName = "Acme, Inc.";
			target.LastName = "foobar!";
			target.Email = "email@acme.com";

			Assert.IsFalse( target.IsValid, "Should be invalid" );
		}
	}
}