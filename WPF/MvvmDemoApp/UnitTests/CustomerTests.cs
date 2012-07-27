using DemoApp.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CustomerTests
    {
        #region Boilerplate Code

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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        #endregion // Boilerplate Code

        [TestMethod]
        public void TestIsValidPerson()
        {
            Customer target = Customer.CreateNewCustomer();
            target.IsCompany = false;
            target.FirstName = "Lloyd";
            target.LastName = "Christmas";
            target.Email = "lloyd@acme.com";

            Assert.IsTrue(target.IsValid, "Should be valid");
        }

        [TestMethod]
        public void TestIsValidCompany()
        {
            Customer target = Customer.CreateNewCustomer();
            target.IsCompany = true;
            target.FirstName = "Acme, Inc.";
            target.Email = "email@acme.com";

            Assert.IsTrue(target.IsValid, "Should be valid");
        }

        [TestMethod]
        public void TestIsInvalidPersonIfEmpty()
        {
            Customer target = Customer.CreateNewCustomer();

            Assert.IsFalse(target.IsValid, "Should be invalid");
        }

        [TestMethod]
        public void TestIsInvalidCompanyIfLastNameExists()
        {
            Customer target = Customer.CreateNewCustomer();
            target.IsCompany = true;
            target.FirstName = "Acme, Inc.";
            target.LastName = "foobar!";
            target.Email = "email@acme.com";

            Assert.IsFalse(target.IsValid, "Should be invalid");
        }
    }
}