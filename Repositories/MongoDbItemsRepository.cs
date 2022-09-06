using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;

        public MongoDbItemsRepository(IMongoclient mongoclient)
        {
            IMongoDatabase database = mongo
        }
        
        public void CreateItem(Item item)
        {

        } 

        public void DeleteItem(Guid id)
        {
            
        }

        public Item GetItem(Guid id)
        {
            
        }

        public IEnumerable<Item> GetItems()
        {
            return items;
        }
    }
    
}