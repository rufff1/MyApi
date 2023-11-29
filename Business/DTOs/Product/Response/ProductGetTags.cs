using Business.DTOs.Category.Response;
using Business.DTOs.Tag.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Product.Response
{
    public class ProductGetTags 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }


       
        
        public List<TagResponseDTO> Tags { get; set; }
        public ProductFindCategoryDTO Category{ get; set; }

    }
}
