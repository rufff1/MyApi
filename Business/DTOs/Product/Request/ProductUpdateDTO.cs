using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Product.Request
{
    public class ProductUpdateDTO
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
