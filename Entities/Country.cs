using System.ComponentModel.DataAnnotations;

namespace Entities
{
    /// <summary>
    /// Domain model for country
    /// Not explose the Domain model for controller and presentation layer that is reason we are implement the 
    /// DTO(Data Transfer Object)
    /// </summary>
    public class Country
    {
        [Key]
        public Guid  CountryID { get; set; }    

        public string? CountryName { get; set; }

    }
}
