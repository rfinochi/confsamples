using System.Data.Entity;

namespace MoneySample.Models
{
    public class MoneyContext : DbContext
    {
        public DbSet<Dollar> Dollars
        {
            get;
            set;
        }
    }
}