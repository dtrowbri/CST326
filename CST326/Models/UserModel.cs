using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CST326.Models
{
    public class UserModel
    {
       
        public int UserId { get; set; }
        
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 200, MinimumLength = 1)]
        public string Password { get; set; }

    }
}