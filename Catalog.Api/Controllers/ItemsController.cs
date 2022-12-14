using System;
using System.Collections.Generic;
using Catalog.Api .Repositories;
using Catalog.Api .Entities;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Catalog.Api .Dtos;
using System.Threading.Tasks;


namespace Catalog.Api.Controllers
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
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
           var items = (await repository.GetItemsAsync())
                        .Select(item=>item.AsDto());
           return items;
        }

        //GET/items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if(item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        //rule for create is first create and return the created item
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await repository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync),new {id = item.Id},item.AsDto());

        }
        [HttpPut("{id}")]
        //the convention of update is not to return anything
        public async Task<ActionResult> UpdateItemAsync(Guid id,UpdateItemDto itemDto)
        {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            await repository.UpdateItemAsync(updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = await repository.GetItemAsync(id);
            if(existingItem is null)
            {
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();
            
        }
    }
}