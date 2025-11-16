using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public record BasketItemDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        [Range(1,double.MaxValue,ErrorMessage ="Price Must Be Greater Than Zero")]
        public decimal Price { get; set; }
        [Range(1,99,ErrorMessage = "Quantity Must Be At Least One Item")]
        public int Quantity { get; set; }
    }
}
