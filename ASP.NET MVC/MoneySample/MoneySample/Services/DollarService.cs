using System.Linq;

using MoneySample.Models;

namespace MoneySample.Services
{
    public class DollarService : IDollarService
    {
        public Dollar GetSavings( string userId )
        {
            using ( MoneyContext context = new MoneyContext( ) )
            {
                return ( from dollar in context.Dollars
                         where dollar.Id == userId
                         select dollar ).Single( );
            }
        }
    }
}