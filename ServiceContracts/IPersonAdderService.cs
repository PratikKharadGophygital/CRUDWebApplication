
using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{

    /// <summary>
    /// Represent the business logic for mainuplating person entity 
    /// </summary>
    public interface IPersonAdderService
    {
        /// <summary>
        /// Add new person
        /// </summary>
        /// <param name="personRequest"> Person details </param>
        /// <returns> Return the same person details with newly generated id </returns>
         Task<PersonResponse> AddPerson(PersonAddRequest? personRequest);

      
    }
}
