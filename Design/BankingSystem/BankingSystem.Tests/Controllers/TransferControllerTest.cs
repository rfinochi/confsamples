using BankingSystem.Controllers;
using BankingSystem.Models;
using System.Web.Mvc;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BankingSystem.Tests.Controllers
{
    [TestClass]
    public class TransferControllerTest
    {
        [TestMethod]
        public void TransferMoneyOk( )
        {
            TransferController controller = new TransferController( new FakeAccountRepository( ) );

            TransferMoneyViewModel model = new TransferMoneyViewModel
                                            {
                                                OriginAccountId = 1,
                                                DestinationAccountId = 2,
                                                AmmountToTransfer = 100
                                            };
 
            ViewResult result = controller.TransferMoney( model ) as ViewResult;

            Assert.AreEqual( "Transferencia exitosa", result.ViewBag.Result );
        }

        [TestMethod]
        public void TransferMoneyOk2( )
        {
            Mock<IAccountRepository> mockRepository = new Mock<IAccountRepository>( );

            mockRepository.Setup( m => m.Get( 1 ) ).Returns( new Account( ) { Id = 1, Balance = 100 } );
            mockRepository.Setup( m => m.Get( 2 ) ).Returns( new Account( ) { Id = 2, Balance = 500 } );

            TransferController controller = new TransferController( mockRepository.Object );

            TransferMoneyViewModel model = new TransferMoneyViewModel
            {
                OriginAccountId = 1,
                DestinationAccountId = 2,
                AmmountToTransfer = 100
            };

            ViewResult result = controller.TransferMoney( model ) as ViewResult;

            Assert.AreEqual( "Transferencia exitosa", result.ViewBag.Result );
        }


        [TestMethod]
        public void TransferMoneyOriginAccountNotFound( )
        {
            TransferController controller = new TransferController( new FakeAccountRepository( ) );

            TransferMoneyViewModel model = new TransferMoneyViewModel
            {
                OriginAccountId = 1000,
                DestinationAccountId = 2,
                AmmountToTransfer = 100
            };

            ViewResult result = controller.TransferMoney( model ) as ViewResult;

            Assert.AreEqual( "La cuenta de origen no existe", result.ViewBag.Result );
        }

        [TestMethod]
        public void TransferMoneyDestinationAccountNotFound( )
        {
            TransferController controller = new TransferController( new FakeAccountRepository( ) );

            TransferMoneyViewModel model = new TransferMoneyViewModel
            {
                OriginAccountId = 1,
                DestinationAccountId = 2000,
                AmmountToTransfer = 100
            };

            ViewResult result = controller.TransferMoney( model ) as ViewResult;

            Assert.AreEqual( "La cuenta de destino no existe", result.ViewBag.Result );
        }

        [TestMethod]
        public void TransferMoneyInsufficientsFunds( )
        {
            TransferController controller = new TransferController( new FakeAccountRepository( ) );

            TransferMoneyViewModel model = new TransferMoneyViewModel
            {
                OriginAccountId = 1,
                DestinationAccountId = 2,
                AmmountToTransfer = 10000
            };

            ViewResult result = controller.TransferMoney( model ) as ViewResult;

            Assert.AreEqual( "La cuenta de origen no tiene fondos suficientes", result.ViewBag.Result );
        }
    }
}
