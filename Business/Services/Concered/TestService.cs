
using AutoMapper;
using Business.DTOs.Common;
using Business.DTOs.Test;
using Business.Services.Abtraction;
using Common.Entities;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using DataAccess.UnitOfWork;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concered
{

  
    public class TestService : ITestService
    {

        public readonly IMapper _mapper;
        public readonly ITestRepository _testRepository;
        public readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TestService> _logger;
        public TestService(ILogger<TestService> logger, IMapper mapper,ITestRepository testRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

    

        public async Task<Response> CreateAsync(TestCreateDTO model)
        {
            

            var tagMap = _mapper.Map<Test>(model);

            Test test = new Test();


            test.SetTest(model.Name);
         

            await _testRepository.CreateAsync(test);
          
            await _unitOfWork.CommitAsync();

            _logger.LogInformation("update oldu test");
            return new Response()
            {
                Message = "ugurla yaradildi"
            };

        }
    }
}
