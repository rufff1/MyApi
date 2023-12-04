using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Entities
{
    public class Team :BaseEntity 
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public byte Age {  get; set; }
        public string Info { get; set; }
        public string Image {  get; set; }
        [Column("email")]
        [Unicode(false)]
        public string? Email { get; set; }


        public int PositionId {  get; set; }
        public virtual TeamPosition Position { get; set; }


     

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
