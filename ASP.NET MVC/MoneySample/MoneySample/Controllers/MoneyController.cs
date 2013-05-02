using System.Web.Mvc;

using MoneySample.Models;
using MoneySample.Services;

namespace MoneySample.Controllers
{
    public class MoneyController : Controller
    {
        public IDollarService _dollarService;

        public MoneyController( IDollarService dollarService )
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