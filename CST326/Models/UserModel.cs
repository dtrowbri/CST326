﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST326.Models
{
    public class UserModel
    {
       
        public int UserId { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}