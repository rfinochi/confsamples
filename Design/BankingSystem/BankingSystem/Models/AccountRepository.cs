using System.Linq;

namespace BankingSystem.Models
{
    public class AccountRepository : IAccountRepository
    {
        public Account Get( int id )
        {
            using ( BankingSystemContext context = new BankingSystemContext( ) )
            {
                return ( from a in context.Accounts
                         where a.Id == id
                         select a ).SingleOrDefault( );
            }
        }

        public void Save( int id, decimal balance )
        {
            using ( BankingSystemContext context = new BankingSystemContext( ) )
            {
                Account account = ( from a in context.Accounts
                                    where a.Id == id
                                    select a ).SingleOrDefault( );

                if ( account != null )
                {
                    account.Balance = balance;

                    context.SaveChanges( );
                }
            }
        }
    }
}