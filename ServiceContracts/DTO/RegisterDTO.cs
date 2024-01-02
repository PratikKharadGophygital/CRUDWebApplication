using Microsoft.AspNetCore.Mvc;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="PersonName is required")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter the proper email address")]
        // Remote : This create the run time javascript code and call the action method this is async request send when user enter the value in email fild. Just declare the controller name and action method name  This is the validation also make sure install this nuget package [AspNetCore ViewFeatures]
        [Remote(action: "IsEmailAlreadtRegistered",controller: "Account", ErrorMessage = "Email is already use")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression("^[0-9]*$",ErrorMessage ="Only number is allowed")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "ConfirmPassword is required")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Password and confirm password do not match")]
        public string ConfirmPassword { get; set; }

        public UserTypeOptions UserType { get; set; } = UserTypeOptions.User;
      
    }
}
