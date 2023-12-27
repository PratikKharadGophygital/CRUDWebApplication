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
    public class PersonUpdaterService : IPersonUpdaterService
    {
        // Private field
        private readonly ApplicationDbContext _db;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonUpdaterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonUpdaterService(IPersonsRepository personsRepository, ILogger<PersonUpdaterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;

        }


        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest != null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            // validation 
            ValidationHelper.ModelValidation(personUpdateRequest);

             Person objPersonUpdateRequest= personUpdateRequest.ToPerson();
            // get matching person object to update 
            Person? matchingPerson = await _personsRepository.UpdatePerson(objPersonUpdateRequest) ;

            if (matchingPerson == null)
            {
                throw new InvalidPersonIDException("Given person id doesn't exist");
            }

            // update all details 

            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Geneder = personUpdateRequest.Geneder.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;

           await _db.SaveChangesAsync();
            return matchingPerson.ToPersonResponse();
        }

    }
}
