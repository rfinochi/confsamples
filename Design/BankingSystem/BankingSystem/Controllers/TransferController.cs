using BankingSystem.Models;
using System;
using System.Web.Mvc;

namespace BankingSystem.Controllers
{
    public class TransferController : Controller
    {
        private IAccountRepository _repository;

        public TransferController( ) : this( new AccountRepository( ) ) { }

        public TransferController( IAccountRepository repository )
        {
            _repository = repository;
        }

        public ActionResult TransferMoney( )
        {
            ViewBag.Result = String.Empty;
            return View( );
        }

        [HttpPost]
        public ActionResult TransferMoney( TransferMoneyViewModel model )
        {
            TransferService service = new TransferService( _repository );

            string result = service.TransferMoney( model.OriginAccountId, model.DestinationAccountId, model.AmmountToTransfer );

            if ( String.IsNullOrEmpty( result ) )
                ViewBag.Result = "Transferencia exitosa";
            else
                ViewBag.Result = result;

            return View( );

        }
    }
}