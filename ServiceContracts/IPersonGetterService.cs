
using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{

    /// <summary>
    /// Represent the business logic for mainuplating person entity 
    /// </summary>
    public interface IPersonGetterService
    {
        

        /// <summary>
        /// Return all person 
        /// </summary>
        /// <returns> Return the list of objects of PersonResponse type </returns>
        Task<List<PersonResponse>> GetAllPersonList();

        /// <summary>
        /// Return the person object based on the given person id 
        /// </summary>
        /// <param name="personID">Person id to search</param>
        /// <returns> Return the matching person object </returns>
        Task<PersonResponse?> GetPersonByPersonID(Guid? personID);

        /// <summary>
        /// Return all the objected mathced with the given search field and search string 
        /// </summary>
        /// <param name="searchBy"> Search field to search </param>
        /// <param name="searchString"> Search string to search 
        /// 
        /// </param>
        Task<List<PersonResponse>?> GetFilteredPerson(string searchBy, string? searchString);

       

      
        /// <summary>
        /// Return persons as csv Download all data in table format
        /// </summary>
        /// <returns></returns>
        Task<MemoryStream> GetPersonCSV();

        /// <summary>
        /// Download the data in excel
        /// </summary>
        /// <returns> Excel file </returns>
        Task<MemoryStream> GetPersonsExcel();

    }
}
