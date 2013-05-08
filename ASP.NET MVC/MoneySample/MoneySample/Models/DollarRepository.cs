using System.Linq;

namespace MoneySample.Models
{
    public class DollarRepository : IDollarRepository
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