using System.Collections.Generic;
using System.Linq;

using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
    public class DbTodoRepository : ITodoRepository
    {
	    public IEnumerable<Item> AllItems
        {
            get
            {
                using ( TodoContext context = new TodoContext( ) )
                {
                    return ( from i in context.Items
                                select i ).ToList( );
                }
            }
        }

        public void Init()
        {
            using ( TodoContext context = new TodoContext( ) )
            {
                context.Database.ExecuteSqlCommand("CREATE TABLE Items (Id integer, Title varchar(150), IsDone integer);");
                
                context.Items.Add(new Item { Id = 1, Title = "Task 1", IsDone = true });
                context.Items.Add(new Item { Id = 2, Title = "Task 2", IsDone = false });
                context.Items.Add(new Item { Id = 3, Title = "Task 3", IsDone = true });
                
                context.SaveChanges();
            }
        }
        
        public Item GetById( int id )
        {
            using ( TodoContext context = new TodoContext( ) )
            {
                return ( from i in context.Items
                         where i.Id == id
                         select i ).FirstOrDefault( );
            }
        }

        public void Add( Item item )
        {
            using ( TodoContext context = new TodoContext( ) )
            {
                item.Id = 1 + this.AllItems.Max( x => ( int? )x.Id ) ?? 0;
                context.Items.Add( item );

                context.SaveChanges( );
            }
        }

        public bool Delete( int id )
        {
            using ( TodoContext context = new TodoContext( ) )
            {
                Item item = ( from i in context.Items
                              where i.Id == id
                              select i ).FirstOrDefault( );
                if ( item == null )
                {
                    return false;
                }
                else
                {
                    context.Items.Remove( item );

                    context.SaveChanges( );

                    return true;
                }
            }
        }
    }
}