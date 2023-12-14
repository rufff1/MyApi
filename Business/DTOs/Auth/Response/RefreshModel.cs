using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Auth.Response
{
    public class RefreshModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }

       
    }
}
