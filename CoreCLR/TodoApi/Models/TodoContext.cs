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

        protected override void OnConfiguring(EntityOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=tcp:j7ptr7bfa1.database.windows.net,1433;Database=Todo;User ID=user1@j7ptr7bfa1;Password=Password11;Trusted_Connection=False;Encrypt=True;Connection Timeout=30;");
        }
    }
}
