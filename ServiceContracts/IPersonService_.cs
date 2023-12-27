
using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{

    /// <summary>
    /// Represent the business logic for mainuplating person entity 
    /// </summary>
    public interface IPersonService_
    {
        /// <summary>
        /// Add new person
        /// </summary>
        /// <param name="personRequest"> Person details </param>
        /// <returns> Return the same person details with newly generated id </returns>
         Task<PersonResponse> AddPerson(PersonAddRequest? personRequest);

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
        /// Return sorted list of persons
        /// </summary>
        /// <param name="allPersons"> Represents list of persons to sort</param>
        /// <param name="sortBy"> Name of the property(key),based on which the persons should be sorted </param>
        /// <param name="sortOrder">ASCE OR DESC</param>
        /// <returns> Return the list of asc or desc oder</returns>
        Task<List<PersonResponse>?> GetSortedPersons(List<PersonResponse> allPersons, string? sortBy,SortOrderOptions sortOrder);

        /// <summary>
        ///  Update the details about the person
        /// </summary>
        /// <param name="personRequest"> Required detail about the person </param>
        /// <returns> Return the PersonID when person successful update the records</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personRequest);

        /// <summary>
        /// Delete a person 
        /// </summary>
        /// <param name="PersonID"></param>
        /// <returns> Return the true or false value base on operation</returns>
        public Task<bool> DeletePerson(Guid? PersonID);

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

        /// <summary>
        /// Upload countries from excel file into database
        /// </summary>
        /// <param name="formFile"> excel file with list of  countries </param>
        /// <returns> Returns number of countries added </returns>
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}
