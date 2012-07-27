using DynamicDataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;
using System.Linq;
using System.Data.SqlServerCe;
using Microsoft.CSharp.RuntimeBinder;

namespace ZPod.DynamicDataAccess.Test
{


    /// <summary>
    ///This is a test class for DynamicConnectionTest and is intended
    ///to contain all DynamicConnectionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DynamicConnectionTest
    {
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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Dispose
        ///</summary>
        [TestMethod()]
        public void DisposeTest()
        {
            IDbConnection realConnection = GetConnection();
            IRecordMapper[] mappers = null; // TODO: Initialize to an appropriate value
            DynamicConnection target = new DynamicConnection(realConnection, mappers); // TODO: Initialize to an appropriate value
            target.Dispose();
        }

        /// <summary>
        ///A test for DynamicConnection Constructor
        ///</summary>
        [TestMethod()]
        public void DynamicConnectionConstructorTest()
        {
            IDbConnection realConnection = GetConnection();
            IRecordMapper[] mappers = null;
            DynamicConnection target = new DynamicConnection(realConnection, mappers);
        }

        /// <summary>
        ///A test for DynamicConnection Constructor
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DynamicConnectionConstructorTest2()
        {
            IDbConnection realConnection = null;
            IRecordMapper[] mappers = null;
            DynamicConnection target = new DynamicConnection(realConnection, mappers);
        }

        private IDbConnection GetConnection()
        {
            return new SqlCeConnection(@"Data Source=..\..\..\Test\TestDB.sdf");
        }

        /// <summary>
        ///A test for QueryValue
        ///</summary>
        [TestMethod()]
        public void QueryValueTest()
        {
            IDbConnection realConnection = GetConnection();
            IRecordMapper[] mappers = null;
            DynamicConnection target = new DynamicConnection(realConnection, mappers);
            string query = "SELECT Count(*) FROM Country";
            Parameter[] parameters = null;
            object expected = 3;
            object actual;
            actual = target.QueryValue(query, parameters);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Query
        ///</summary>
        [TestMethod()]
        public void QueryTest()
        {
            IDbConnection realConnection = GetConnection();
            IRecordMapper[] mappers = null;
            DynamicConnection target = new DynamicConnection(realConnection, mappers);
            string query = "SELECT Name AS CountryName, * FROM Country";
            object[] parameters = null;

            dynamic result = target.Query(query, parameters);

            string[] countries = new string[3] { "Argentina", "Chile", "Uruguay" };
            int cant = 0;
            foreach (var country in result)
            {
                countries.Any(c => c == country[0]);
                countries.Any(c => c == country.Nameo);
                cant++;
            }

            Assert.AreEqual(cant, 3);
        }

        /// <summary>
        ///A test for Query
        ///</summary>
        [TestMethod()]
        public void QueryTestMapped()
        {
            IDbConnection realConnection = GetConnection();
            DynamicConnection target = new DynamicConnection(realConnection, new CountryNameMapper(), new SquareBracketsMapper());
            string query = "SELECT Name FROM Country WHERE Name = 'Argentina'";
            object[] parameters = null;

            dynamic result = target.Query(query, parameters);

            foreach (var country in result)
            {
                Assert.AreEqual("[Country Name: Argentina]", country.Name);
            }
        }

        /// <summary>
        ///A test for Query
        ///</summary>
        [TestMethod()]
        public void QueryNotExistantField()
        {
            IDbConnection realConnection = GetConnection();
            DynamicConnection target = new DynamicConnection(realConnection, new CountryNameMapper(), new SquareBracketsMapper());
            string query = "SELECT * FROM Country";
            object[] parameters = null;

            dynamic result = target.Query(query, parameters);

            foreach (var country in result)
            {
                Assert.IsNull(country.FakeField);
                break;
            }
        }

        /// <summary>
        ///A test for Query
        ///</summary>
        [TestMethod()]
        public void QueryNotExtraField()
        {
            IDbConnection realConnection = GetConnection();
            DynamicConnection target = new DynamicConnection(realConnection, new CountFieldMapper());
            string query = "SELECT Id, Name FROM Country";
            object[] parameters = null;

            dynamic result = target.Query(query, parameters);

            foreach (var country in result)
            {
                Assert.AreEqual(2, country.FieldCount);
            }
        }

        /// <summary>
        ///A test for Query
        ///</summary>
        [TestMethod()]
        public void QueryParameter()
        {
            IDbConnection realConnection = GetConnection();
            DynamicConnection target = new DynamicConnection(realConnection, new CountFieldMapper());
            string query = "SELECT Id, Name FROM Country WHERE Name = @0";

            dynamic result = target.Query(query, "Argentina");

            bool hasRecords = false;
            foreach (var country in result)
            {
                hasRecords = true;
                Assert.AreEqual<string>(country.Name, "Argentina");
            }

            Assert.IsTrue(hasRecords);
        }

        /// <summary>
        ///A test for Query
        ///</summary>
        [TestMethod()]
        public void QueryIndex()
        {
            IDbConnection realConnection = GetConnection();
            DynamicConnection target = new DynamicConnection(realConnection, new CountFieldMapper());
            string query = "SELECT Name FROM Country WHERE Name = @0";

            dynamic result = target.Query(query, "Argentina");

            bool hasRecords = false;
            foreach (var country in result)
            {
                hasRecords = true;
                Assert.AreEqual<string>(country[0], "Argentina");
            }

            Assert.IsTrue(hasRecords);
        }

        [TestMethod()]
        public void QueryStringIndex()
        {
            IDbConnection realConnection = GetConnection();
            DynamicConnection target = new DynamicConnection(realConnection, new CountFieldMapper());
            string query = "SELECT Name FROM Country WHERE Name = @0";

            dynamic result = target.Query(query, "Argentina");

            bool hasRecords = false;
            foreach (var country in result)
            {
                hasRecords = true;
                Assert.AreEqual<string>(country["Name"], "Argentina");
            }

            Assert.IsTrue(hasRecords);
        }

        [TestMethod()]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void QueryInvalidIndex()
        {
            IDbConnection realConnection = GetConnection();
            DynamicConnection target = new DynamicConnection(realConnection, new CountFieldMapper());
            string query = "SELECT Name FROM Country WHERE Name = @0";

            dynamic result = target.Query(query, "Argentina");

            foreach (var country in result)
            {
                var val = country[2.4];
            }
        }

        /// <summary>
        ///A test for Query
        ///</summary>
        [TestMethod()]
        public void QueryValueParameter()
        {
            IDbConnection realConnection = GetConnection();
            DynamicConnection target = new DynamicConnection(realConnection, new CountFieldMapper());
            string query = "SELECT Id FROM Country WHERE Id = @0";

            dynamic result = target.QueryValue(query, 1);

            Assert.AreEqual<int>(1, result);
        }
    }
}
