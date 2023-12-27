using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using RepositoryContracts;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace Services
{
    public class PersonAdderService : IPersonAdderService
    {
        // Private field
        private readonly ApplicationDbContext _db;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonAdderService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonAdderService(IPersonsRepository personsRepository, ILogger<PersonAdderService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
            //  _db = personsDbContext;
            //_countriesService = countriesService;
            #region
            //if (initialize)
            //{
            //    //  e4cf4f6a - 4813 - 4444 - 976e-b8e36ef591db
            //    //  627b761a - 6a01 - 4494 - 9209 - cbaef66be99b
            //    // 3424dd75 - e1fe - 49db - bb5d - 07845a064df1
            //    // 6f533b27 - 88f1 - 4c7c - b41d - dfa5da74f408
            //    // 2b050098 - aa44 - 44b5 - 9d36 - f76c2fe4b091
            //    //fc627ff7 - 1311 - 4a04 - bfb7 - e2478573b93e

            //    _db.Add(new Person()
            //    {

            //        PersonID = Guid.Parse("131381a8-9589-4632-b03a-f02f85785816"),
            //        PersonName = "atual",
            //        Email = "PersonName1@gmail.com",
            //        DateOfBirth = DateTime.Parse("1993-01-02"),Geneder="male",Address = "street address 1",
            //        ReceivedNewsLetter = false,CountryID=Guid.Parse("2b712f00-0bbf-4cca-80b3-0f8ebd03253d"),
            //    });

            //    _db.Add(new Person()
            //    {

            //        PersonID = Guid.Parse("62915349-5768-4994-8122-1093e8158232"),
            //        PersonName = "pratik",
            //        Email = "pratik@gmail.com",
            //        DateOfBirth = DateTime.Parse("1993-01-02"),
            //        Geneder = "male",
            //        Address = "street address 2",
            //        ReceivedNewsLetter = false,
            //        CountryID = Guid.Parse("e3016761-d84a-4800-b9d8-63e8ce93231b"),
            //    });

            //    _db.Add(new Person()
            //    {

            //        PersonID = Guid.Parse("e856c662-3324-4f50-b047-b92a06328241"),
            //        PersonName = "shubham",
            //        Email = "shubham@gmail.com",
            //        DateOfBirth = DateTime.Parse("1993-01-02"),
            //        Geneder = "male",
            //        Address = "street address 3",
            //        ReceivedNewsLetter = false,
            //        CountryID = Guid.Parse("29bf1dcf-cfcc-42c0-b017-0d6443f2bfd6"),
            //    });


            //    _db.Add(new Person()
            //    {

            //        PersonID = Guid.Parse("4a805771-d79a-4477-adc7-e2842f0d7472"),
            //        PersonName = "sandesh",
            //        Email = "sandesh@gmail.com",
            //        DateOfBirth = DateTime.Parse("1993-01-02"),
            //        Geneder = "male",
            //        Address = "street address 4",
            //        ReceivedNewsLetter = false,
            //        CountryID = Guid.Parse("788f0a8e-ff73-41ad-9d16-4a04200738a2"),
            //    });


            //    _db.Add(new Person()
            //    {

            //        PersonID = Guid.Parse("944b2d87-823d-4d5b-9886-f5002632845c"),
            //        PersonName = "mayur",
            //        Email = "mayur@gmail.com",
            //        DateOfBirth = DateTime.Parse("1993-01-02"),
            //        Geneder = "male",
            //        Address = "street address 5",
            //        ReceivedNewsLetter = false,
            //        CountryID = Guid.Parse("3d926d70-baf3-4a8a-9113-3cd14ad3e5ef"),
            //    });
            //    _db.Add(new Person()
            //    {

            //        PersonID = Guid.Parse("b819de98-587e-408b-965c-38464b5d7d57"),
            //        PersonName = "sharad",
            //        Email = "sharad@gmail.com",
            //        DateOfBirth = DateTime.Parse("1993-01-02"),
            //        Geneder = "male",
            //        Address = "street address 6",
            //        ReceivedNewsLetter = false,
            //        CountryID = Guid.Parse("3d926d70-baf3-4a8a-9113-3cd14ad3e5ef"),
            //    });
            //}
            #endregion
        }

        
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personRequest)
        {
            if (personRequest == null) 
            {
                throw new ArgumentNullException(nameof(personRequest));
            }
            
            // Model validation
            ValidationHelper.ModelValidation(personRequest);
                
            // convert PersonAddRequest to Person Type
            Person person = personRequest.ToPerson();

            // Generate the person ID 
            person.PersonID = Guid.NewGuid();

            // Add person object to persons list
            Person persons = await  _personsRepository.AddPerson(person);
        

           // _db.sp_InsertPerson(person);

            // common return type method call
            return persons.ToPersonResponse();

        
        }

    }
}
