
using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{

    /// <summary>
    /// Represent the business logic for mainuplating person entity 
    /// </summary>
    public interface IPersonDeleterService
    {
        

        /// <summary>
        /// Delete a person 
        /// </summary>
        /// <param name="PersonID"></param>
        /// <returns> Return the true or false value base on operation</returns>
        public Task<bool> DeletePerson(Guid? PersonID);

        
    }
}
