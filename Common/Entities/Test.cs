using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Test : BaseEntity
    {

        public string Name { get; private set; }
        public string Image { get;set; }


        public void SetTest(string name)
        {
            Name = name;
        
        }

    }

 



}
