using Microsoft.VisualStudio.TestTools.UnitTesting;

using DemoApp.DataAccess;
using DemoApp.Model;
using DemoApp.ViewModel;

namespace UnitTests
{
    [TestClass]
    public class AllCustomersViewModelTests
    {
        [TestMethod]
        public void TestTotalSelectedSales()
        {
            CustomerRepository repos = new CustomerRepository(Constants.CUSTOMER_DATA_FILE);
            AllCustomersViewModel target = new AllCustomersViewModel(repos);

            int notifications = 0;
            target.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "TotalSelectedSales")
                    ++notifications;
            };

            Assert.AreEqual(0.0, target.TotalSelectedSales, "Should be zero when no customers are selected");

            CustomerViewModel firstCust = target.AllCustomers[0];

            firstCust.IsSelected = true;
            Assert.AreEqual(1, notifications, "TotalSelectedSales change notification was not raised");
            Assert.AreEqual(firstCust.TotalSales, target.TotalSelectedSales);

            CustomerViewModel secondCust = target.AllCustomers[1];
            secondCust.IsSelected = true;
            Assert.AreEqual(2, notifications, "TotalSelectedSales change notification was not raised again");
            Assert.AreEqual(firstCust.TotalSales + secondCust.TotalSales, target.TotalSelectedSales);
        }

        [TestMethod]
        public void TestNewCustomerIsAdded()
        {
            CustomerRepository repos = new CustomerRepository(Constants.CUSTOMER_DATA_FILE);
            AllCustomersViewModel target = new AllCustomersViewModel(repos);

            Assert.AreEqual(3, target.AllCustomers.Count, "Test data includes three customers");

            repos.AddCustomer(Customer.CreateCustomer(123.45, "new", "customer", false, "new.customer@email.com"));

            Assert.AreEqual(4, target.AllCustomers.Count, "Adding a customer to the repository increase the Count");
        }
    }
}