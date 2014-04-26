namespace BankingSystem.Models
{
    public interface IAccountRepository
    {
        Account Get( int id );

        void Save( int id, decimal balance );
    }
}