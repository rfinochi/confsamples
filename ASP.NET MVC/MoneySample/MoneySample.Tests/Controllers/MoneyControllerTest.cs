using System.Web.Mvc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using MoneySample.Controllers;
using MoneySample.Models;

namespace MoneySample.Tests.Controllers
{
    [TestClass]
    public class MoneyControllerTest
    {
        [TestMethod]
        public void Times( )
        {
            Mock<IDollarRepository> mockDollarService = new Mock<IDollarRepository>( );

            mockDollarService.Setup( mds => mds.GetSavings( "rodolfof" ) ).Returns( new Dollar( ) { Id = "rodolfof", Amount = 5 } );

            MoneyController controller = new MoneyController( mockDollarService.Object );

            ViewResult result = controller.Times( "rodolfof", 2 ) as ViewResult;

            Assert.AreEqual( 10, ( (Dollar)result.Model ).Amount );
        }
    }
}