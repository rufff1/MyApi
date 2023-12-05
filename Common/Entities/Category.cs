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
    public class Category : BaseEntity
    {

       
        public string Name { get; set; }

       
        public string Image {  get; set; }


       public List<Product> Products { get; set; }
        public List<Blog> Blogs { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get;set; }    

      

    }
}
