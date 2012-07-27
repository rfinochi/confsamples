using System.Linq;
using System.Windows.Data;

using DemoApp.Properties;
using DemoApp.ViewModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
	[TestClass]
	public class MainWindowViewModelTests
	{
		[TestMethod]
		public void TestViewAllCustomers( )
		{
			MainWindowViewModel target = new MainWindowViewModel( Constants.CUSTOMER_DATA_FILE );
			CommandViewModel commandVM = target.Commands.First( cvm => cvm.DisplayName == Strings.MainWindowViewModel_Command_ViewAllCustomers );
			commandVM.Command.Execute( null );

			var collectionView = CollectionViewSource.GetDefaultView( target.Workspaces );
			Assert.IsTrue( collectionView.CurrentItem is AllCustomersViewModel, "Invalid current item." );
		}

		[TestMethod]
		public void TestCreateNewCustomer( )
		{
			MainWindowViewModel target = new MainWindowViewModel( Constants.CUSTOMER_DATA_FILE );
			CommandViewModel commandVM = target.Commands.First( cvm => cvm.DisplayName == Strings.MainWindowViewModel_Command_CreateNewCustomer );
			commandVM.Command.Execute( null );

			var collectionView = CollectionViewSource.GetDefaultView( target.Workspaces );
			Assert.IsTrue( collectionView.CurrentItem is CustomerViewModel, "Invalid current item." );
		}

		[TestMethod]
		public void TestCannotViewAllCustomersTwice( )
		{
			MainWindowViewModel target = new MainWindowViewModel( Constants.CUSTOMER_DATA_FILE );
			CommandViewModel commandVM = target.Commands.First( cvm => cvm.DisplayName == Strings.MainWindowViewModel_Command_ViewAllCustomers );

			commandVM.Command.Execute( null );
			commandVM.Command.Execute( null );

			var collectionView = CollectionViewSource.GetDefaultView( target.Workspaces );
			Assert.IsTrue( collectionView.CurrentItem is AllCustomersViewModel, "Invalid current item." );
			Assert.IsTrue( target.Workspaces.Count == 1, "Invalid number of view models." );
		}

		[TestMethod]
		public void TestCloseAllCustomersWorkspace( )
		{
			MainWindowViewModel target = new MainWindowViewModel( Constants.CUSTOMER_DATA_FILE );

			Assert.AreEqual( 0, target.Workspaces.Count, "Workspaces isn't empty." );

			CommandViewModel commandVM = target.Commands.First( cvm => cvm.DisplayName == Strings.MainWindowViewModel_Command_ViewAllCustomers );

			commandVM.Command.Execute( null );
			Assert.AreEqual( 1, target.Workspaces.Count, "Did not create viewmodel." );

			var allCustomersVM = target.Workspaces[ 0 ] as AllCustomersViewModel;
			Assert.IsNotNull( allCustomersVM, "Wrong viewmodel type created." );

			allCustomersVM.CloseCommand.Execute( null );
			Assert.AreEqual( 0, target.Workspaces.Count, "Did not close viewmodel." );
		}
	}
}