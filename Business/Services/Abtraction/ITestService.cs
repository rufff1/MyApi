using Business.DTOs.Common;
using Business.DTOs.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abtraction
{
    public interface ITestService
    {
        Task<Response> CreateAsync(TestCreateDTO model);
    }
}
