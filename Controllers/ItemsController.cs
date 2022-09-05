using System;
using System.Collections.Generic;
using Catalog.Repositories;
using Catalog.Entities;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Catalog.Dtos;


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
           var items = repository.GetItems().Select(item=>item.AsDto());
           return items;
        }

        //GET/items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = repository.GetItem(id);
            if(item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        //rule for create is first create and return the created item
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem),new {id = item.Id},item.AsDto());

        }

    }
}