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
    public class PersonDeleterService : IPersonDeleterService
    {
        // Private field
        private readonly ApplicationDbContext _db;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonDeleterService> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonDeleterService(IPersonsRepository personsRepository, ILogger<PersonDeleterService> logger, IDiagnosticContext diagnosticContext)
        {
            _personsRepository = personsRepository;
            _logger = logger;
            _diagnosticContext = diagnosticContext;

        }
      
        public async Task<bool> DeletePerson(Guid? PersonID)
        {
            if (PersonID == null)
            {
                throw new ArgumentNullException(nameof(PersonID));
            }

            Person? person = await _db.Persons.FirstOrDefaultAsync(temp => temp.PersonID == temp.PersonID);

            if (person == null)
            {
                return false;
            }

            _db.Persons.Remove( _db.Persons.First( temp => temp.PersonID == PersonID));
            await _db.SaveChangesAsync();

            return true;    

        }

    }
}
