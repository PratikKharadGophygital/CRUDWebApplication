using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1
{
    public class PersonsServiceTest
    {
        //private readonly IPersonService_ _personService;

        // S.O.L.I.D | I principle use here
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonUpdaterService _personUpdaterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonSorterService _personSorterService;

        private readonly ICountriesService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        private readonly IPersonsRepository _personRepository;
        private readonly Mock<IPersonsRepository> _personRepositoryMock;


        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {

            _personRepositoryMock = new Mock<IPersonsRepository>();
            _personRepository = _personRepositoryMock.Object;

            _fixture = new Fixture();
            var countriesInitialData = new List<Country>() { };
            var personInitialData = new List<Person>() { };
            var loggerMock = new Mock<ILogger<PersonGetterService>>();

            DbContextMock<ApplicationDbContext> dbContextMock = new(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object;


            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personInitialData);

            _countryService = new CountriesService(null);
            _personGetterService = new PersonGetterService(_personRepository, loggerMock, null);

            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        // When we supply null value as PersonAddRequest it should throw ArgumentNullException
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            // Arrange 
            PersonAddRequest? personAddRequest = null;

            Func<Task> action = async () =>
            {
                await _personService.AddPerson(personAddRequest);
            };

            // Using Fluent Assertion Nuget package 
            await action.Should().ThrowAsync<ArgumentNullException>();

            // Without Use Fluent Assertion Nuget package
            // Act
            // await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //{

            //});

        }

        // When we supply null value as PersonName it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentNullException()
        {
            // Arrange 
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.PersonName, null as string)
                .Create();

            Person person = personAddRequest.ToPerson();

            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            // Fluenet assertion
            // Func given the return type value that the reason we are use and action method not return the any value 
            Func<Task> action = async () =>
            {
                await _personService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();
            // Act
            // await Assert.ThrowsAsync<ArgumentException>(async () =>
            //{
            //    await _personService.AddPerson(personAddRequest);
            //});

        }

        // 
        [Fact]

        public async Task AddPerson_FullPersonDetails_ToBeSuccessful()
        {
            // Arrange 
            //PersonAddRequest? personAddRequest = new PersonAddRequest()
            //{
            //    PersonName = "Pratik sunil kharad",
            //    Email = "Person@gmail.com",
            //    Address = "Address person details",
            //    CountryID = Guid.NewGuid(),
            //    Geneder = ServiceContracts.Enums.GenderOptions.Male,
            //    DateOfBirth = DateTime.Parse("2000-01-02"),
            //    ReceivedNewsLetter = true

            //};


            // using autofixture create the object autofixture is help to generate the object 
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "exampleofemail@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create();

            Person person = personAddRequest.ToPerson();
            PersonResponse person_response_expected = person.ToPersonResponse();

            _personRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);


            // Act
            PersonResponse personResponse_from_add = await _personService.AddPerson(personAddRequest);

            person_response_expected.PersonID = personResponse_from_add.PersonID;
            //List<PersonResponse> personResponses_list = await _personService.GetAllPersonList();

            // Assert
            // Assert.True(personResponse_from_add.PersonID != Guid.Empty);
            personResponse_from_add.PersonID.Should().NotBe(Guid.Empty);

            //Assert.Contains(personResponse_from_add, personResponses_list);
            //personResponses_list.Should().Contain(personResponse_from_add);

        }

        #endregion

        #region GetPersonByPersonID

        // If we supply null as personID if should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            // Arrange
            Guid? personID = null;

            PersonResponse? new_person_response = await _personService.GetPersonByPersonID(personID);

            // Assert
            //Assert.Null(new_person_response);
            new_person_response.Should().BeNull();

        }

        // If we supply a valid person id it should  return the valid person details as PersonResponse object 
        [Fact]
        public async Task GetPersonByPersonID_WithPersonID()
        {
            // Arrange
            CountryAddRequest? country_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response = await _countryService.AddCountry(country_request);

            PersonAddRequest personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(temp => temp.Email, "exampleofemail@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create();
            //new PersonAddRequest()
            //{
            //    PersonName = "person",
            //    Email = "email@gamil.com",
            //    CountryID = country_response.CountryID,
            //    DateOfBirth = DateTime.Parse("2000-01-01"),
            //    Geneder = GenderOptions.Male,
            //    ReceivedNewsLetter = true
            //};
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(personResponse.PersonID);


            // Assert
            //   Assert.Equal(personResponse, person_response_from_get);
            person_response_from_get.Should().Be(personResponse);



        }

        #endregion

        #region GetAllPerson

        [Fact]
        public async Task GetAllPerson_ToBeEmptyList()
        {
            var persons = new List<Person>();
            _personRepositoryMock.Setup(temp => temp.GetAllPerson()).ReturnsAsync(persons);
            // Act
            List<PersonResponse> person_from_get = await _personService.GetAllPersonList();

            // Assert
            //Assert.Empty(person_from_get);
            person_from_get.Should().BeEmpty();
        }

        // First, we will add few person; and then when we call GetAllPerson(), it should return the same person that were added 
        public async Task GetAllPerson_AddFewPersons_ToBeSuccessful()
        {
            // Arrange
            List<Person> persons = new List<Person>() {
                _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail1@gmail.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail2@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create(),


             _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail3@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create()
            };



            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //foreach (PersonAddRequest person in person_request)
            //{
            //    PersonResponse personResponse = await _personService.AddPerson(person);
            //    person_response_list_from_add.Add(personResponse);
            //}

            //// Act
            List<PersonResponse> person_responses_from_get = await _personService.GetAllPersonList();

            ////foreach (PersonResponse person_responses_from_add in person_response_list_from_add)
            ////{
            ////    //Assert
            ////    Assert.Contains(person_responses_from_add, person_responses_from_get);
            ////}
            //person_responses_from_get.Should().BeEquivalentTo(person_response_list_from_add);

            _testOutputHelper.WriteLine("Expedted :");
            // Print person_responses_list_from_add
            foreach (PersonResponse person_responses_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_responses_from_add.ToString());
            }

            _testOutputHelper.WriteLine("Atual :");
            foreach (PersonResponse person_responses_from_gets in person_responses_from_get)
            {

                _testOutputHelper.WriteLine(person_responses_from_gets.ToString());
            }

        }

        #endregion

        #region GetFilteredPerson
        // If the search text is empty and search by is 'PersonName' it should return all person
        [Fact]
        public async Task GetFilteredPerson_EmptySearchText()
        {
            List<Person> persons = new List<Person>() {
                _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail1@gmail.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail2@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create(),


             _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail3@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create()
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);
            // Act
            List<PersonResponse>? person_responses_from_search = await _personService.GetFilteredPerson(nameof(Person.PersonName), "");

            //foreach (PersonResponse person_responses_from_add in person_response_list_from_add)
            //{
            //    //Assert
            //    Assert.Contains(person_responses_from_add, person_responses_from_search);
            //}

            person_responses_from_search.Should().BeEquivalentTo(person_response_list_expected);

            _testOutputHelper.WriteLine("Expedted :");
            // Print person_responses_list_from_add
            foreach (PersonResponse person_responses_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_responses_from_add.ToString());
            }

            _testOutputHelper.WriteLine("Atual :");
            foreach (PersonResponse person_responses_from_gets in person_responses_from_search)
            {

                _testOutputHelper.WriteLine(person_responses_from_gets.ToString());
            }
            person_responses_from_search.Should().BeEquivalentTo(person_response_list_expected);
        }

        // First we will add few persons; and then we will search based on person name with some search string. it should return the matching persons
        [Fact]
        public async Task GetFilteredPerson_SearchByPersonName()
        {
            List<Person> persons = new List<Person>() {
                _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail1@gmail.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail2@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create(),


             _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail3@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create()
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            _personRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
                .ReturnsAsync(persons);
            // Act
            List<PersonResponse>? person_responses_from_search = await _personService.GetFilteredPerson(nameof(Person.PersonName), "sa");

            //foreach (PersonResponse person_responses_from_add in person_response_list_from_add)
            //{
            //    //Assert
            //    Assert.Contains(person_responses_from_add, person_responses_from_search);
            //}

            person_responses_from_search.Should().BeEquivalentTo(person_response_list_expected);

            _testOutputHelper.WriteLine("Expedted :");
            // Print person_responses_list_from_add
            foreach (PersonResponse person_responses_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_responses_from_add.ToString());
            }

            _testOutputHelper.WriteLine("Atual :");
            foreach (PersonResponse person_responses_from_gets in person_responses_from_search)
            {

                _testOutputHelper.WriteLine(person_responses_from_gets.ToString());
            }
            person_responses_from_search.Should().BeEquivalentTo(person_response_list_expected);


        }
        #endregion

        #region GetSortedPersons

        // When we sort based on PersonName in DES, it should return Persons list in descending on personame
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            // Arrange

            List<Person> persons = new List<Person>() {
                _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail1@gmail.com")
                .With(temp => temp.Country, null as Country)
                .Create(),

            _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail2@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create(),


             _fixture.Build<Person>()
                .With(temp => temp.Email, "exampleofemail3@gmail.com")
                .With(temp => temp.Address, "some one address street address area ")
                .Create()
            };

            List<PersonResponse> person_response_list_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            _personRepositoryMock.Setup(temp => temp.GetAllPerson()).ReturnsAsync(persons);
         
            // Act
            List<PersonResponse> person_responses_from_search = await _personService.GetFilteredPerson(nameof(Person.PersonName), "");

    
            _testOutputHelper.WriteLine("Expedted :");
            // Print person_responses_list_from_add
            foreach (PersonResponse person_responses_from_add in person_response_list_expected)
            {
                _testOutputHelper.WriteLine(person_responses_from_add.ToString());
            }

            List<PersonResponse> allPersons = await _personService.GetAllPersonList();

            List<PersonResponse>? person_responses_from_sort = await _personService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            _testOutputHelper.WriteLine("Atual :");
            foreach (PersonResponse person_responses_from_gets in person_responses_from_sort)
            {

                _testOutputHelper.WriteLine(person_responses_from_gets.ToString());
            }

            //person_responses_from_sort.Should().BeEquivalentTo(person_response_list_from_add);
            person_responses_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);





        }

        #endregion

        #region UpdatePerson

        // When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerSon_ToBeArgumentNullException()
        {
            // Arrange 
            PersonUpdateRequest? person_update_request = null;

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(person_update_request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        // When PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            // Arrange


            Person person = _fixture.Build<Person>()
            .With(temp => temp.PersonName, "Smith")
            .With(temp => temp.Email, "exampleofemail1@gmail.com")
            .With(temp => temp.Address, "some one address street address area ")
            .With(temp => temp.Geneder, "Male")
            .With(temp => temp.Country, null as Country )            
            .Create();

            PersonResponse person_response_from_add = person.ToPersonResponse();
            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

            _personRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
                    .ReturnsAsync(person);


   

            person_update_request.PersonName = null;

            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(person_update_request);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
            // Assert
            //Assert.Throws<ArgumentException>(() =>
            //{

            //    _personService.UpdatePerson(person_update_request);
            //});
            //PersonUpdateRequest person_update_request = new PersonUpdateRequest() { 
        }

        // First, add a new person and try to update the person 
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation_ToBeArgumentException()
        {
            // Arrange

            Person  person = _fixture.Build<Person>()
           .With(temp => temp.PersonName,  null as string)
           .With(temp => temp.Email, "exampleofemail1@gmail.com")
           .With(temp => temp.Address, "some one address street address area ")
           .With(temp => temp.Country, null as Country)
           .With(temp => temp.Geneder, "Male")
           .Create();

            PersonResponse person_response_from_add = person.ToPersonResponse();
            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

            _personRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>()))
                    .ReturnsAsync(person);

            _personRepositoryMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(person);
            // Act
            PersonResponse person_response_from_update = await _personService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personService.GetPersonByPersonID(person_response_from_update.PersonID);

            // Assert
            Assert.Equal(person_response_from_update, person_response_from_get);
            //PersonUpdateRequest person_update_request = new PersonUpdateRequest() { 
            person_response_from_update.Should().Be(person_response_from_get);


        }

        #endregion

        #region DeletePerson


        [Fact]
        public async Task DeletePerson_ValidPersonID_ToBeSuccessful()
        {

            // Arrange 


            Person person = _fixture.Build<Person>()
           .With(temp => temp.Email, "exampleofemail1@gmail.com")
           .With(temp => temp.Address, "some one address street address area ")
           .With(temp => temp.Country, null as Country)
           .With(temp => temp.Geneder, "Male")
           .Create(); 

            PersonResponse person_response_from_add = person.ToPersonResponse();

            // Act
            _personRepositoryMock
               .Setup(temp => temp.DeletePersonByPersonID(It.IsAny<Guid>()))
               .ReturnsAsync(true);

            _personRepositoryMock
                .Setup(temp => temp.GetPersonByPersonID(It.IsAny<Guid>()))
                .ReturnsAsync(person);


            bool isDeleted = await _personService.DeletePerson(person_response_from_add.PersonID);

            // Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();


        }


        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {

            // Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            // Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeFalse();


        }
        #endregion

    }
}