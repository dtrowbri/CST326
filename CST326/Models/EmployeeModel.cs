using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CST326.Models
{
    public class EmployeeModel
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Admin { get; set; }

        public EmployeeModel(int employeeId, string firstName, string lastName, string phoneNumber, string email, int admin)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Admin = admin;
        }

        public EmployeeModel()
        {
        }
    }
}