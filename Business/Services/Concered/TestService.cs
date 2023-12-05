using AutoMapper;
using Business.DTOs.Common;
using Business.DTOs.Test;
using Business.Services.Abtraction;
using Common.Entities;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using DataAccess.UnitOfWork;
using DocumentFormat.OpenXml.Spreadsheet;
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

        public TestService(IMapper mapper,ITestRepository testRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
        }

    

        public async Task<Response> CreateAsync(TestCreateDTO model)
        {
            

            var tagMap = _mapper.Map<Test>(model);

            Test test = new Test();


            test.SetTest(model.Name);
         

            await _testRepository.CreateAsync(test);
          
            await _unitOfWork.CommitAsync();

            return new Response()
            {
                Message = "ugurla yaradildi"
            };

        }
    }
}
