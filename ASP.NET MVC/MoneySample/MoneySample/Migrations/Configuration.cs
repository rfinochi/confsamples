using System.Data.Entity.Migrations;

using MoneySample.Models;

namespace MoneySample.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MoneyContext>
    {
        public Configuration( )
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed( MoneyContext context )
        {
            Dollar dollar1 = new Dollar { Id = "rodolfof", Amount = 5 };
            context.Dollars.AddOrUpdate( d => d.Id, dollar1 );
        }
    }
}