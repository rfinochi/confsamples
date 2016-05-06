using Microsoft.EntityFrameworkCore;

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
            optionsBuilder.UseSqlServer("Server=tcp:XXXX,1433;Database=Todo;User ID=XXXX;Password=XXXX;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}