using System;
using System.Collections.Generic;
using Catalog.Repositories;
using Catalog.Entities;
using Microsoft.AspNetCore.Mvc;


namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly InMemItemsRepository repository;

        public ItemsController(){
            repository = new InMemItemsRepository();

        }

        //HTTPGET tells when someone do GET/items, down method is going to react
        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
           var items = repository.GetItems();
           return items;
        }

        //GET/items/{id}
        [HttpGet("{id}")]
        public Item GetItem(Guid id)
        {
            var item = repository.GetItem(id);
            return item;
        }

    }
}