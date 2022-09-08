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
        public IEnumerable<ItemDto> GetItemsAsync()
        {
           var items = repository.GetItemsAsync().Select(item=>item.AsDto());
           return items;
        }

        //GET/items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItemAsync(Guid id)
        {
            var item = repository.GetItemAsync(id);
            if(item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        //rule for create is first create and return the created item
        public ActionResult<ItemDto> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync),new {id = item.Id},item.AsDto());

        }
        [HttpPut("{id}")]
        //the convention of update is not to return anything
        public ActionResult UpdateItemAsync(Guid id,UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItemAsync(id);
            if(existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            repository.UpdateItemAsync(updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItemAsync(Guid id)
        {
            var existingItem = repository.GetItemAsync(id);
            if(existingItem is null)
            {
                return NotFound();
            }
            repository.DeleteItemAsync(id);
            return NoContent();
            
        }
    }
}