using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating country entity 
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Add a country object to the list of countries 
        /// </summary>
        /// <param name="countryAddRequest">country object to add</param>
        /// <returns>return the newly generated country id </returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);


        /// <summary>
        /// Return all countries list 
        /// </summary>
        /// <param name="countryAddRequest">all countries list </param>
        /// <returns>return the all countries list </returns>
        Task<List<CountryResponse>> GetAllCountryList();

        /// <summary>
        /// Return the country object based on the country id 
        /// </summary>
        /// <param name="countryID">CountryID (GUID) search</param>
        /// <returns>Matching country as CountryResponse</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID); 
    }


}