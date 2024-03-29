﻿using AutoFixture;
using CRUDWebApplication.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    public class PersonControllerTest
    {
        private readonly  IPersonService_ _personService;

        

        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonControllerTest> _logger;

        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonUpdaterService _personUpdaterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonSorterService _personSorterService;

        private readonly Mock<ICountriesService> _countryServiceMock;
        private readonly Mock<IPersonService_> _personServiceMock;
        private readonly Mock<ILogger<PersonControllerTest>> _loggerMock;

        private readonly Fixture _fixture;

        public PersonControllerTest()
        {
              _fixture = new Fixture();
            _loggerMock = new Mock<ILogger<PersonControllerTest>>();
            _countryServiceMock = new Mock<ICountriesService>();
            _personServiceMock = new Mock<IPersonService_>();

             _countriesService = _countryServiceMock.Object;
            _personService = _personServiceMock.Object;
            _logger = _loggerMock.Object;

        }

        #region Index 

        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            // Arrange 
            List<PersonResponse> person_response_list = _fixture.Create<List<PersonResponse>>();

            PersonsController personsController = new PersonsController(_personAdderService, _personUpdaterService, _personDeleterService, _personGetterService, _personSorterService, _countriesService,null);

            _personServiceMock
                .Setup(temp => temp.GetFilteredPerson(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(person_response_list);

            _personServiceMock
                .Setup(temp => temp.GetSortedPersons
                (It.IsAny<List<PersonResponse>>(), It.IsAny<string>(),
                It.IsAny<SortOrderOptions>()))
                .ReturnsAsync(person_response_list);

            IActionResult result  = await personsController.Index(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<SortOrderOptions>());

            // Assert 
           ViewResult viewResult =  Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            viewResult.ViewData.Model.Should().Be(person_response_list);
        
        
        }
        #endregion

        #region

        [Fact]
        public async void Create_IFModelError_ToReturnCreateView()
        {
            // Arrange 
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries= _fixture.Create<List<CountryResponse>>();

        

            _countryServiceMock
                .Setup(temp => temp.GetAllCountryList())
                .ReturnsAsync(countries);    


            _personServiceMock
            .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(person_response);

            PersonsController personsController = new PersonsController(_personAdderService, _personUpdaterService, _personDeleterService, _personGetterService, _personSorterService, _countriesService,null);

            // Act
            personsController.ModelState.AddModelError("PersonName", "Person Name Can't Be Blank");

            IActionResult result = await personsController.Create(person_add_request);

            // Assert 
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(person_add_request);
        }


        [Fact]
        public async void Create_IfNoModelError_ToReturnRedirectToIndex()
        {
            // Arrange 
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countryServiceMock
                .Setup(temp => temp.GetAllCountryList())
                .ReturnsAsync(countries);


            _personServiceMock
            .Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
            .ReturnsAsync(person_response);

            PersonsController personsController = new PersonsController(_personAdderService, _personUpdaterService, _personDeleterService, _personGetterService, _personSorterService, _countriesService,null);

            // Act
            personsController.ModelState.AddModelError("PersonName", "Person Name Can't Be Blank");

            IActionResult result = await personsController.Create(person_add_request);

            // Assert 
            RedirectToActionResult  redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion



    }
}
