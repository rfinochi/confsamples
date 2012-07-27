using System;
using System.ComponentModel;

using DemoApp.DataAccess;
using DemoApp.Model;
using DemoApp.Properties;
using DemoApp.ViewModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class CustomerViewModelTests
	{
		[TestMethod]
		public void TestCustomerType( )
		{
			Customer cust = Customer.CreateNewCustomer( );
			CustomerRepository repos = new CustomerRepository( Constants.CUSTOMER_DATA_FILE );
			CustomerViewModel viewModel = new CustomerViewModel( cust, repos );

			viewModel.CustomerType = Strings.CustomerViewModel_CustomerTypeOption_Company;
			Assert.IsTrue( cust.IsCompany, "Should be a company" );

			viewModel.CustomerType = Strings.CustomerViewModel_CustomerTypeOption_Person;
			Assert.IsFalse( cust.IsCompany, "Should be a person" );

			viewModel.CustomerType = Strings.CustomerViewModel_CustomerTypeOption_NotSpecified;
			string error = ( viewModel as IDataErrorInfo )[ "CustomerType" ];
			Assert.IsFalse( String.IsNullOrEmpty( error ), "Error message should be returned" );
		}
	}
}