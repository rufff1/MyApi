using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTOs.Common
{
    public class Response
    {
        public List<string> Errors { get; set; }
        public string Message { get; set; }

        
     
    }
    public class Response<T> : Response
    {
        public T Data { get; set; }
    }
}
