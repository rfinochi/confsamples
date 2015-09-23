using Microsoft.Data.Entity;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<Item> Items
        {
            get;
            set;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:XXXX.database.windows.net,1433;Database=Todo;User ID=XXXX@XXXX;Password=XXXX;Trusted_Connection=False;Encrypt=False;Connection Timeout=30;");
        }
    }
}
