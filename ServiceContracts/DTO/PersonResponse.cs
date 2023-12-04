

using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represent the DTO class that is used as return type of most methods of Person services 
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        [Required(ErrorMessage = "Person name can't be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email name can't be blank")]
        [EmailAddress(ErrorMessage ="Email value should be a valid email")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Geneder { get; set; }
        [Required(ErrorMessage = "Please select the country")]
        public Guid? CountryID { get; set; }
        public string? CountryName { get; set; }

        [Required(ErrorMessage = "Address can't be blank")]
        public string?  Address { get; set; }
        public bool ReceivedNewsLetter { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compare the current object data with parameter object 
        /// </summary>
        /// <param name="obj"> The PersonResponse object to compare </param>
        /// <returns> True or False, indicating whether all person details are matched with the specified parameter object </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) { return false; }

            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;
            return this.PersonID == person.PersonID
                && this.PersonName == person.PersonName
                && this.Email == person.Email
                && this.DateOfBirth == person.DateOfBirth
                && this.Geneder == person.Geneder
                && this.CountryID == person.CountryID
                && this.CountryName == person.CountryName
                && this.Address == person.Address
                && this.Age == person.Age
                && this.ReceivedNewsLetter == person.ReceivedNewsLetter;

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        
        public override string ToString()
        {
            return $"PersonID : {PersonID}, PersonName: { PersonName},Email : {Email} ,DateOfBirth : {DateOfBirth?.ToString("dd mm yyyy")} ,Geneder:{Geneder} ,CountryID :{CountryID},Address:{Address} ,ReceivedNewsLetter :{ReceivedNewsLetter} ,Age : {Age} ";
        }

        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Geneder = (GenderOptions)Enum.Parse(typeof(GenderOptions), Geneder, true),
                CountryID = CountryID,
                Address = Address,
                ReceivedNewsLetter = ReceivedNewsLetter
            };

        }

    }
    
    /// <summary>

    /// Extension method inject into person class
    /// </summary>
    public static class PersonExtensions
    {
        /// <summary>
        ///  An etension method to convert an obbv object of person class into PersonResponse class
        /// </summary>
        /// <param name="person"> The person object toconvert. </param>
        /// <returns> Return the converted PersonResponse object. </returns>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            // person => PersonResponse
            return new PersonResponse()
            {
                 PersonID = person.PersonID
                ,PersonName = person.PersonName
                ,Email = person.Email
                ,DateOfBirth = person.DateOfBirth
                ,Geneder = person.Geneder
                ,CountryID = person.CountryID
                ,Address = person.Address                
                ,ReceivedNewsLetter = person.ReceivedNewsLetter
                ,Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,
                 CountryName = person.Country?.CountryName
            };
        }
    }


}
