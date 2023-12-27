
using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{

    /// <summary>
    /// Represent the business logic for mainuplating person entity 
    /// </summary>
    public interface IPersonUpdaterService
    {
        

        /// <summary>
        ///  Update the details about the person
        /// </summary>
        /// <param name="personRequest"> Required detail about the person </param>
        /// <returns> Return the PersonID when person successful update the records</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personRequest);

        
    }
}
