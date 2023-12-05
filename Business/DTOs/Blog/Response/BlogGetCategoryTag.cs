using Business.DTOs.Category.Response;
using Business.DTOs.Tag.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Blog.Response
{
    public class BlogGetCategoryTag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }



        public List<TagResponseDTO> Tags { get; set; }
        public ProductFindCategoryDTO Category { get; set; }

    }
}
