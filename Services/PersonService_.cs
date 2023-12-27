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
    public class PersonService_ : IPersonService_
    {
        // Private field
        private readonly ApplicationDbContext _db;
        private readonly ICountriesService _countriesService;
        private readonly IPersonsRepository _personsRepository;
        private readonly ILogger<PersonService_> _logger;
        private readonly IDiagnosticContext _diagnosticContext;

        public PersonService_(IPersonsRepository personsRepository, ILogger<PersonService_> logger, IDiagnosticContext diagnosticContext)
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

        // Create the common code when need the return type of personResonse 
        //private PersonResponse ConvertPersonToPersonResponse(Person? person)
        //{   
        //    PersonResponse personResponse = person.ToPersonResponse();
        //    personResponse.CountryName = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
        //    return personResponse;
        //}

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

        public async Task<List<PersonResponse>> GetAllPersonList()
        {
            _logger.LogInformation("GetAllPersonList in PersonService");
            /*var persons = await _personsRepository.Include("Country").ToListAsync(); */// Include with use the properti define name
            var persons = await _personsRepository.GetAllPerson();
            return persons
                .Select(temp => temp.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;

            Person? person = await _personsRepository.GetPersonByPersonID(personID);

            if (person == null) return null;

            return person.ToPersonResponse();                

        }

        public async Task<List<PersonResponse>?> GetFilteredPerson(string searchBy, string? searchString)
        {
            _logger.LogInformation("GetPersonByPersonID of Personservice ");
            List<PersonResponse> allPersons = await GetAllPersonList();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }

            using (Operation.Time("GetFilteredPerson of Person Service")) { 
            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(temp => (temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString("dd mmm yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Geneder):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Geneder) ? temp.Geneder.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.CountryID):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.CountryName) ? temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingPersons = allPersons; break;

            }
        }
            _diagnosticContext.Set("Persons", allPersons);

            return matchingPersons;
        }

        public async Task<List<PersonResponse>?> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
            switch
            { 
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Geneder), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Geneder, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Geneder), SortOrderOptions.DESC) => allPersons.OrderBy(temp => temp.Geneder, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.DESC) => allPersons.OrderBy(temp => temp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceivedNewsLetter), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceivedNewsLetter).ToList(),

                (nameof(PersonResponse.ReceivedNewsLetter), SortOrderOptions.DESC) => allPersons.OrderBy(temp => temp.ReceivedNewsLetter).ToList(),

                _ =>  allPersons
            };

            return sortedPersons;      
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

        //public Task<MemoryStream> GetPersonCSV()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<MemoryStream> GetPersonCSV()
        {
            try
            
             {
                MemoryStream memoryStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memoryStream);

                CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
                // Frist parameter is file second parameter default asp.net core culture, third parameter is pass every compleation of start to end file writing
                CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);
                // write the header PersonID,PersonName as like
                csvWriter.WriteField(nameof(PersonResponse.PersonName));
                csvWriter.WriteField(nameof(PersonResponse.Email));
                csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
                csvWriter.WriteField(nameof(PersonResponse.Age));
                csvWriter.WriteField(nameof(PersonResponse.Geneder));
                csvWriter.WriteField(nameof(PersonResponse.CountryName));
                csvWriter.WriteField(nameof(PersonResponse.Address));
                csvWriter.WriteField(nameof(PersonResponse.ReceivedNewsLetter));
                //csvWriter.WriteHeader<PersonResponse>();
                // write th new line
                csvWriter.NextRecord();

                List<PersonResponse> persons = _db.Persons
                    .Include("Country")
                    .Select(  temp =>  temp.ToPersonResponse()).ToList();

                foreach (PersonResponse person in persons)
                {
                    csvWriter.WriteField(person.PersonName);
                    csvWriter.WriteField(person.Email);
                    if (person.DateOfBirth.HasValue)
                    {
                        csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-mm-dd"));
                    }
                    else
                    {
                        csvWriter.WriteField("");
                    }
                    
                    csvWriter.WriteField(person.Age);
                    csvWriter.WriteField(person.CountryName);
                    csvWriter.WriteField(person.Address);
                    csvWriter.WriteField(person.ReceivedNewsLetter);
                    csvWriter.NextRecord();
                    csvWriter.Flush();
                }

                //await csvWriter.WriteRecordsAsync(persons);
                // MemoryStream hold the pervious values we want to start from begging 
                memoryStream.Position = 0;
                return memoryStream;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<MemoryStream> GetPersonsExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using(ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                worksheet.Cells["A1"].Value = "Person Name";
                worksheet.Cells["B1"].Value = "Email";
                worksheet.Cells["C1"].Value = "Date of birth";
                worksheet.Cells["D1"].Value = "Age";
                worksheet.Cells["E1"].Value = "Gender";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Address";
                worksheet.Cells["H1"].Value = "Receive News Letters";

                using(ExcelRange headerCells = worksheet.Cells["A1:H1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                List<PersonResponse> persons = _db.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();

                foreach (PersonResponse person  in persons)
                {
                    worksheet.Cells[row, 1].Value = person.PersonName;
                    worksheet.Cells[row, 2].Value = person.Email;
                    if(person.DateOfBirth.HasValue)
                    worksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("yyyy-MM-dd");

                    worksheet.Cells[row, 4].Value = person.Age;
                    worksheet.Cells[row, 5].Value = person.Geneder;
                    worksheet.Cells[row, 6].Value = person.CountryName;
                    worksheet.Cells[row, 7].Value = person.Address;
                    worksheet.Cells[row, 8].Value = person.ReceivedNewsLetter;

                    row++;
                }

                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;

        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = worksheet.Dimension.Rows;
                
                for (int row = 2; row <= rowCount; row++)
                {
                    string? cellValue = Convert.ToString(worksheet.Cells[row,1].Value);

                    if (!string.IsNullOrEmpty(cellValue))
                    {
                        string? countryName = cellValue;

                        if(_db.Countries.Where(temp => temp.CountryName == countryName).Count() == 0)
                        {
                            Country country = new Country()
                            {
                                CountryName = countryName
                            };
                            _db.Countries.Add(country);
                            await _db.SaveChangesAsync();

                            countriesInserted++;

                        }
                    }
                }
            }
            return countriesInserted;

        }
    }
}
