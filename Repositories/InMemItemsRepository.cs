using System;
using System.Linq;
using System.Collections.Generic;
using Catalog.Entities;

namespace Catalog.Repositories
{
    public class InMemItemsRepository
    {
        private readonly List<Item> items = new()
        {
            new Item{Id = Guid.NewGuid(),Name = "Potion",Price = 9,CreatedDate = DateTimeOffset.UtcNow},
            new Item{Id = Guid.NewGuid(),Name = "Iron Sword",Price = 20,CreatedDate = DateTimeOffset.UtcNow},
            new Item{Id = Guid.NewGuid(),Name = "Bronze Shield",Price = 18,CreatedDate = DateTimeOffset.UtcNow},
        };

        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        public Item GetItem(Guid Id)
        {
            return items.where(item=>item.id == id).SingleOrDefault(); 
            //the output is a collection but we need only one item or the default one thats why we use SingleOrDefault
            
        }
    }
}