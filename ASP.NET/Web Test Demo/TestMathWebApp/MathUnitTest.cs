using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestMapWebApp
{
    [TestClass]
    public class MathUnitTest
    {
        [TestMethod]
        public void SumTest( )
        {
            Assert.AreEqual( 2, MathWebApp.Math.Sum( 1, 1 ) );
        }
    }
}