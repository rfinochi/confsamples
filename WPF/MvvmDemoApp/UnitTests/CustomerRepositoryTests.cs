using DemoApp.DataAccess;
using DemoApp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CustomerRepositoryTests
    {
        #region Boilerplate Code

        public CustomerRepositoryTests()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #endregion // Boilerplate Code

        [TestMethod]
        public void TestAllCustomersAreLoaded()
        {
            CustomerRepository target = new CustomerRepository(Constants.CUSTOMER_DATA_FILE);
            Assert.AreEqual(3, target.GetCustomers().Count, "Invalid number of customers in repository.");
        }

        [TestMethod]
        public void TestNewCustomerIsAddedProperly()
        {
            CustomerRepository target = new CustomerRepository(Constants.CUSTOMER_DATA_FILE);            
            Customer cust = Customer.CreateNewCustomer();

            bool eventArgIsValid = false;
            target.CustomerAdded += (sender, e) => eventArgIsValid = (e.NewCustomer == cust);
            target.AddCustomer(cust);

            Assert.IsTrue(eventArgIsValid, "Invalid NewCustomer property");
            Assert.IsTrue(target.ContainsCustomer(cust), "New customer was not added");
        }
    }
}