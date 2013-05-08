using System.Web.Mvc;

using MoneySample.Models;

namespace MoneySample.Controllers
{
    public class MoneyController : Controller
    {
        public IDollarRepository _dollarService;

        public MoneyController( IDollarRepository dollarService )
        {
            _dollarService = dollarService;
        }

        public ActionResult Times( string userId, int multiplier )
        {
            Dollar dollar = _dollarService.GetSavings( userId );

            dollar.Amount *= multiplier;

            return View( dollar );
        }
    }
}