using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {

        public string Token { get; set; }
        public string Username { get; set; }
    }

    [Table("Users")]
    public class SignUpRequest
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int type { get; set; }
        public bool linkedin { get; set; }
        public bool google { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public bool? stepWizard { get; set; }
    }
    public class ChangePasswordRequest
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
    [Table("UserAllow")]
    public class UserAllow
    {
        [Key]
        public int id { get; set; }
        public string sender { get; set; }
        public string receiver { get; set; }
    }
    public class UserAllowReq
    {

        public string from { get; set; }
        public string to { get; set; }
    }


    public class ResetPasswordRequestModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ResetLink { get; set; }
    }
    [Table("customerStripe")]
    public class customerStripe
    {
        [Key]
        public int id { get; set; }
        public string customer_stripe_id { get; set; }
        public int userid { get; set; }
        public int type { get; set; }
    }
}

