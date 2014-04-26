using System.Data.Entity.Migrations;

using BankingSystem.Models;

namespace BankingSystem.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BankingSystemContext>
    {
        public Configuration( )
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed( BankingSystemContext context )
        {
            Account account1 = new Account { Id = 1, Balance = 100 };
            context.Accounts.AddOrUpdate( a => a.Id, account1 );

            Account account2 = new Account { Id = 2, Balance = 500 };
            context.Accounts.AddOrUpdate( a => a.Id, account2 );
        }
    }
}