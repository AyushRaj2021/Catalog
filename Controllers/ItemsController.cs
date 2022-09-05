using System;
using System.Collections.Generic;
using Catalog.Repositories;
using Catalog.Entities;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository){
            this.repository = repository;
        }

        //HTTPGET tells when someone do GET/items, down method is going to react
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
           var items = repository.GetItems().Select(item=> new ItemDto
           {
                Id = item.Id,
                Name = item.Name;
                Price=item.Price;
                CreatedDate = item.CreatedDate;
           });
           return items;
        }

        //GET/items/{id}
        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {
            var item = repository.GetItem(id);
            if(item is null)
            {
                return NotFound();
            }
            return item;
        }

    }
}