using System;
using System.Linq;
using System.Collections.Generic;
using Catalog.Entities;

namespace Catalog.Repositories
{
    

    public class InMemItemsRepository : IItemsRepository
    {
        private readonly List<Item> items = new()
        {
            new Item{Id = Guid.NewGuid(),Name = "Potion",Price = 9,CreatedDate = DateTimeOffset.UtcNow},
            new Item{Id = Guid.NewGuid(),Name = "Iron Sword",Price = 20,CreatedDate = DateTimeOffset.UtcNow},
            new Item{Id = Guid.NewGuid(),Name = "Bronze Shield",Price = 18,CreatedDate = DateTimeOffset.UtcNow},
        };

        public IEnumerable<Item> GetItemsAsync()
        {
            return items;
        }

        public Item GetItemAsync(Guid id)
        {
            return items.Where(item=>item.Id == id).SingleOrDefault(); 
            //the output is a collection but we need only one item or the default one thats why we use SingleOrDefault
            
        }
        public void CreateItemAsync(Item item)
        {
            items.Add(item);
        }
        public void UpdateItemAsync(Item item)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;
        }

        public void DeleteItemAsync(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
        }

    }
}