using Business.DTOs.Category.Response;
using Business.DTOs.Tag.Response;
using Common.Constants.Product;
using Common.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Business.DTOs.Product.Response
{
    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }


        [JsonIgnore]
        public ProductFindCategoryDTO Product { get; set; }

       


    }
}
