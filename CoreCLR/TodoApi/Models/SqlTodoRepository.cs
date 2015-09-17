using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Microsoft.Data.Entity;

namespace TodoApi.Models
{
    public class SqlTodoRepository : ITodoRepository
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
