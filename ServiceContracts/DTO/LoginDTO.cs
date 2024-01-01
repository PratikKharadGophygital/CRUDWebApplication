using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage="Email cant't be blank")]
        [EmailAddress(ErrorMessage = "Email should be in a proper email address format")]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
