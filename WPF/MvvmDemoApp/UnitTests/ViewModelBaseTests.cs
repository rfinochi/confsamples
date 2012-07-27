using System;
using DemoApp.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ViewModelBaseTests
    {
        #region Boilerplate Code

        public ViewModelBaseTests()
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
        public void TestPropertyChangedIsRaisedCorrectly()
        {
            TestViewModel target = new TestViewModel();
            bool eventWasRaised = false;
            target.PropertyChanged += (sender, e) => eventWasRaised = e.PropertyName == "GoodProperty";
            target.GoodProperty = "Some new value...";
            Assert.IsTrue(eventWasRaised, "PropertyChanged event was not raised correctly.");
        }

        [TestMethod]
        public void TestExceptionIsThrownOnInvalidPropertyName()
        {
            TestViewModel target = new TestViewModel();
            try
            {
                target.BadProperty = "Some new value...";
#if DEBUG
                Assert.Fail("Exception was not thrown when invalid property name was used in DEBUG build.");
#endif
            }
            catch (Exception)
            {
#if !DEBUG
                Assert.Fail("Exception was thrown when invalid property name was used in RELEASE build.");
#endif
            }
        }

        private class TestViewModel : ViewModelBase
        {
            protected override bool ThrowOnInvalidPropertyName
            {
                get { return true; }
            }

            public string GoodProperty
            {
                get { return null; }
                set 
                { 
                    base.OnPropertyChanged("GoodProperty");
                }
            }

            public string BadProperty
            {
                get { return null; }
                set
                {
                    base.OnPropertyChanged("ThisIsAnInvalidPropertyName!");
                }
            }
        }
    }
}