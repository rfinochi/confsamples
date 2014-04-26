using BankingSystem.Models;

namespace BankingSystem.Tests.Controllers
{
    public class FakeAccountRepository : IAccountRepository
    {
        public Account Get( int id )
        {
            if ( id == 1 )
                return new Account { Id = 1, Balance = 100 };
            else if ( id == 2 )
                return new Account { Id = 2, Balance = 500 };
            else
                return null;
        }

        public void Save( int id, decimal balance )
        {
        }
    }
}
