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
        [HttpPut("{id}")]
        //the convention of update is not to return anything
        public ActionResult UpdateItem(Guid id,UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItem(id);
            if(existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            repository.UpdateItem(updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = repository.GetItem(id);
            if(existingItem is null)
            {
                return NotFound();
            }
            repository.DeleteItem(id);
            return NoContent();
            
        }
    }
}