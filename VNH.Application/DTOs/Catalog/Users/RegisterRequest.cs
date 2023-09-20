using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VNH.Domain.Enums;

namespace VNH.Application.DTOs.Catalog.Users
{
    public class RegisterRequest
    {
        public RegisterRequest(string email, string password, string confirmpassword) {
            Email = email;
            Password = password;
            ConfirmPassword = confirmpassword;
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
