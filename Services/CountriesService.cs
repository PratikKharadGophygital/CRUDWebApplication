using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        // private field 
        private readonly IContriesRepository _contriesRepository;

        public CountriesService(IContriesRepository contriesRepository)
        {
            _contriesRepository = contriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            // Validation : countryAddRequest parameter can't be null 
            if (countryAddRequest==null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            // Validation : CountryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            // Validation : CountryName can't be duplicated
            if (await _contriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Given country name already exists");

            }

            // Convert object from countryaddrequest to country type 
            // dto into to domain model convert object 
            Country country = countryAddRequest.ToCountry();

            // Generate countryID 
            country.CountryID = Guid.NewGuid();

            // Add country object into country list 
            await _contriesRepository.AddCountry(country);

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountryList()
        {
           return  (await _contriesRepository.GetAllCountries())
                .Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null)
            {
                return null;
            }

            Country? country_respons_from_list = await _contriesRepository.GetCountryByCountryID(countryID.Value);

            if (country_respons_from_list == null)
            {
                return null;
            }

            return country_respons_from_list.ToCountryResponse();
        }
    }
}