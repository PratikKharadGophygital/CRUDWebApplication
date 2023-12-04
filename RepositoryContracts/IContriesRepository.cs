using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing countries 
    /// </summary>
    public interface IContriesRepository
    {
        /// <summary>
        /// Add the new country object to the data store 
        /// </summary>
        /// <param name="country">country object to add </param>
        /// <returns>Returns the country object after adding it to the data store</returns>
        Task<Country> AddCountry(Country country);
        Task<List<Country>> GetAllCountries();
        Task<Country?> GetCountryByCountryID(Guid countryID);
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}