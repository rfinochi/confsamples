using System.Collections.Generic;

namespace TodoApi.Models
{
    public interface ITodoRepository
    {
        IEnumerable<Item> AllItems { get; }
        void Add( Item item );
        Item GetById( int id );
        bool Delete( int id );
    }
}
