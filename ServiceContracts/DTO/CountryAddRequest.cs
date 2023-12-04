

using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class for adding a new country 
    /// </summary>
    public class CountryAddRequest
    {
        public string? CountryName { get; set; }
        public Guid CountryID { get; set; }

        /// <summary>
        /// Convert CountryAddRequest object into country class
        /// </summary>
        /// <returns></returns>
        public Country ToCountry()
        {
            return new Country() { CountryName = CountryName };
        }
    }
}
