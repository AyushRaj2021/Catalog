using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Api.Dtos
{
    public class CreateItemDto
    {
        [Required]
        public String Name { get; init; }

        [Required]
        [Range(1,1000)]
        public decimal Price { get; init; }
    }
}