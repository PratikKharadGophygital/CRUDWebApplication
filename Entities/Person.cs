

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    /// <summary>
    /// Person is domain class
    /// </summary>
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }
        [StringLength(40)] // nvarchar (40) default nvarchar value is max so define the string length 
        public string? PersonName { get; set; }
        [StringLength(50)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Geneder { get; set; }
        public Guid? CountryID { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }
        // bit
        public bool ReceivedNewsLetter { get; set; }
        public string? PhoneNumber { get; set; }
        [ForeignKey("CountryID")]
        public virtual Country? Country { get; set; }

        public override string ToString()
        {
            return $"PersonID : {PersonID}, PersonName: { PersonName},Email : {Email} ,DateOfBirth : {DateOfBirth?.ToString("dd mm yyyy")} ,Geneder:{Geneder} ,CountryName :{Country?.CountryName},Address:{Address} ,ReceivedNewsLetter :{ReceivedNewsLetter}  ";
        }
    }
}
