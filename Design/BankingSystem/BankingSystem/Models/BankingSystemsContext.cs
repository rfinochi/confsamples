using System.Data.Entity;

namespace BankingSystem.Models
{
    public class BankingSystemContext : DbContext
    {
        public DbSet<Account> Accounts
        {
            get;
            set;
        }
    }
}