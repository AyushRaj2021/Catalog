using Catalog.Api .Dtos;
using Catalog.Api .Entities;

namespace Catalog.Api
{
    public static class Extensions
    {
        //using this here, the current item ,can have a method called ASDTO, that returns its identity or version.
        public static ItemDto AsDto(this Item item)
        {
            return  new ItemDto
           {
                Id = item.Id,
                Name = item.Name,
                Price=item.Price,
                CreatedDate = item.CreatedDate
           };
        }
    }
}