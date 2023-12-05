using Common.Constants.Blog;
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
    public class Blog : BaseEntity
    {
        public string Name {  get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public int CategoryId {  get; set; }


        public BlogType BlogType { get; set; }


        public Category Category { get; set; }


        public List<BlogTag> BlogTags { get; set; }



        [NotMapped]
        public IFormFile ImageFile { get; set; }



        [NotMapped]
        [MaxLength(3)]
        public List<int> TagIds { get; set; }
    }
}
