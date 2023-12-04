

using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    /// <summary>
    /// We are use TDD (Test Drive Development) 
    /// We are first write the test case then write the real implemention
    /// </summary>
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public  CountriesServiceTest()
        {
            var countriesInitialData = new List<Country>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            ApplicationDbContext dbContext = dbContextMock.Object;
            _countriesService = new CountriesService(null);
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);

        }

        #region AddCountry
        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            // Arrange
            CountryAddRequest? request = null;


            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(request);
            });
        }

        // When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            }; 

            //Assert
            await Assert.ThrowsAsync<ArgumentException>( async () =>
            {
                await _countriesService.AddCountry(request);
            });
        }

        // When the countryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            // Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };
            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            //Assert
           await Assert.ThrowsAsync<ArgumentException>( async () =>
            {
               await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        // When you supply proper country name, it should insert(add) the country to the existing list of countries
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            // Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };            

            //Act
            CountryResponse response = await _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountryList();

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response,countries_from_GetAllCountries);
        }

        #endregion

        #region  GetAllCountries

        //The list of countries should be empty by default (before adding any countires)
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            // Act
            List<CountryResponse> actual_country_response_list = await _countriesService.GetAllCountryList();

            //Assert
            Assert.Empty(actual_country_response_list);

        }

        [Fact]
        public  async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>() {
                new CountryAddRequest() { CountryName = "usa"},
                new CountryAddRequest() { CountryName = "uk"}

            };

            //Act
            List<CountryResponse> countries_list_from_add_Country = new List<CountryResponse>();
            foreach (CountryAddRequest country_request in country_request_list)
            {
               countries_list_from_add_Country.Add(await _countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualcountryResponse = await _countriesService.GetAllCountryList();

            // read each element from countries 
            foreach (CountryResponse expectec_contry in countries_list_from_add_Country)
            {
                Assert.Contains(expectec_contry,actualcountryResponse);
            }
        }

        #endregion

        #region GetCountryByCountryID

        // if we supply the null countryID then response is null
        [Fact]
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;

            //Act
            CountryResponse countryResponses = await _countriesService.GetCountryByCountryID(countryID);

            //Assert
            Assert.Null(countryResponses);
        }

        // If we supply the valid country id then response is matching countryid
        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            //Arrange 
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "India"
            };

            CountryResponse countryResponse_from_add = await _countriesService.AddCountry(countryAddRequest);

            //Act
           CountryResponse countryResponse_from_get = await _countriesService.GetCountryByCountryID(countryResponse_from_add.CountryID);

            //Assert
            Assert.Equal(countryResponse_from_add, countryResponse_from_get);
        }



        #endregion

    }
}
