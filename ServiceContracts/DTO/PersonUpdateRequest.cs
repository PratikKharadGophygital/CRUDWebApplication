using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonUpdateRequest
    {
        // need to create the common validation method 
        [Required(ErrorMessage = "PersonID can't be blank")]
        public Guid PersonID { get; set; }

        [Required(ErrorMessage = "Person name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Person Email can't blank")]
        [EmailAddress(ErrorMessage = "Email value should be a valid")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Geneder { get; set; }
        public Guid? CountryID { get; set; }

        public string? Address { get; set; }
        public bool ReceivedNewsLetter { get; set; }

        /// <summary>
        /// ToConvert Convert the current object data into a new object of person 
        /// helpful convert the data into specific domain 
        /// </summary>
        /// <returns>Return the PersonAddRequest object to Person object</returns>
        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Geneder = Geneder.ToString(),
                CountryID = CountryID,
                Address = Address,
                ReceivedNewsLetter = ReceivedNewsLetter,
            };
        }
    }
}
