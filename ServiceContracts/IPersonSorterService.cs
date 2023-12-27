
using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{

    /// <summary>
    /// Represent the business logic for mainuplating person entity 
    /// </summary>
    public interface IPersonSorterService
    {
        

        /// <summary>
        /// Return sorted list of persons
        /// </summary>
        /// <param name="allPersons"> Represents list of persons to sort</param>
        /// <param name="sortBy"> Name of the property(key),based on which the persons should be sorted </param>
        /// <param name="sortOrder">ASCE OR DESC</param>
        /// <returns> Return the list of asc or desc oder</returns>
        Task<List<PersonResponse>?> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy,SortOrderOptions sortOrder);

       
    }
}
