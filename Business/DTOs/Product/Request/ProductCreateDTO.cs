

using Common.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.DTOs.Product.Request
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }



      

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        [MaxLength(3)]
        public List<int> TagIds { get; set; }

        
    }
}
