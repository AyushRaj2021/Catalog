using System;
using System.Collections.Generic;
using Catalog.Api .Entities;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
    public interface IItemsRepository
    {
        //task represent asynchronous operation
        Task<Item> GetItemAsync(Guid id);

        Task<IEnumerable<Item>>GetItemsAsync();

        Task CreateItemAsync(Item item);

        Task UpdateItemAsync(Item item); 

        Task DeleteItemAsync(Guid id);
    }
}