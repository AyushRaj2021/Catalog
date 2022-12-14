using System;
using System.Linq;
using System.Collections.Generic;
using Catalog.Api .Entities;
using System.Threading.Tasks;


namespace Catalog.Api.Repositories
{
    

    public class InMemItemsRepository : IItemsRepository
    {
        private readonly List<Item> items = new()
        {
            new Item{Id = Guid.NewGuid(),Name = "Potion",Price = 9,CreatedDate = DateTimeOffset.UtcNow},
            new Item{Id = Guid.NewGuid(),Name = "Iron Sword",Price = 20,CreatedDate = DateTimeOffset.UtcNow},
            new Item{Id = Guid.NewGuid(),Name = "Bronze Shield",Price = 18,CreatedDate = DateTimeOffset.UtcNow},
        };

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(items);
            //return a complete Task with items (as this method is not called so)
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var item = items.Where(item=>item.Id == id).SingleOrDefault(); 
            //the output is a collection but we need only one item or the default one thats why we use SingleOrDefault
            return await Task.FromResult(item);

            
        }
        public async Task CreateItemAsync(Item item)
        {
            items.Add(item);
            await Task.CompletedTask;
            //create some Task that has already completed and return it without returning anything inside it

        }

        public async Task UpdateItemAsync(Item item)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;
            await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
            await Task.CompletedTask;
        }

    }
}