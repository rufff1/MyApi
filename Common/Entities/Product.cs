using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Product :BaseEntity
    {

        public string Name { get; set; }
     
        public string Description { get; set; }
     
        public string Image { get; set; }
        

       public Category Category { get; set; }
        
       public int CategoryId { get; set; }

        public List<ProductTag> ProductTags { get; set; }


        [NotMapped]
        [MaxLength(3)]
        public ICollection<int> TagIds { get; set; } 


        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
