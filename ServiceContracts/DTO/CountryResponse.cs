using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryID { get; set; }

        public string? CountryName { get; set; }

        // It compare the current object to another object of countryResponse type and return true
        // If both values are same; otherwise returns false

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {

                return false;
            }

            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }


            // object is converted into custom object 
            CountryResponse country_to_Compare = (CountryResponse)obj;
            return CountryID == country_to_Compare.CountryID && CountryName == country_to_Compare.CountryName;    
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();  
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName
            };
        }
    }

}
