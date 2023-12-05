using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Tag :BaseEntity
    {
        public string Name {  get; set; }

        public List<ProductTag> ProductTags { get; set; }
        public List<BlogTag> BlogTags { get; set; }


    }
}
