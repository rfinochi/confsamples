using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Models
{
    public class MemoryTodoRepository : ITodoRepository
    {
        private readonly List<Item> _items = new List<Item>()
        {
            new Item { Id = 1, Title = "In Memory Item" }
        };

        public IEnumerable<Item> AllItems
        {
            get
            {
                return _items;
            }
        }

        public Item GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Item item)
        {
            item.Id = 1 + _items.Max(x => (int?)x.Id) ?? 0;
            _items.Add(item);
        }

        public bool Delete(int id)
        {
            var item = GetById(id);
            if (item == null)
                return false;

            _items.Remove(item);
            return true;
        }
    }
}
