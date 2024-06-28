using Api.Contexts;
using Api.Helper;
using Api.Models;

using Microsoft.Extensions.Configuration;

using System;
using System.Linq;

namespace Api.Services
{
    public class AuthService
    {
        DBContextModel _db;
        EmailService _EmailService;
        IConfiguration _Configuration;
        public AuthService(DBContextModel db, EmailService emailService, IConfiguration configuration)
        {
            _db = db;
            _EmailService = emailService;
            _Configuration = configuration;
        }

        public bool IsEmailAvailable(string email) { return _db.SignUpRequest.Any(x => x.email == email); }

        public SignUpRequest SendVerificationEmail(string email)
        {
            var user = _db.SignUpRequest.FirstOrDefault(x => x.email == email);

            if (user == null)
                return null;
            string link = $"{_Configuration["clientBase"]}verifyEmail?d={Common.Encrypt(DateTime.UtcNow.ToString())}&e={Common.Encrypt(email)}";
            if (_EmailService.SendVerificationEmail(email, link))
            {
                if (_EmailService.SendWelcomEmail(email))
                {
                    return user;
                }
            }
            return null;
        }

        public SignUpRequest SendResetEmail(string email)
        {
            var user = _db.SignUpRequest.FirstOrDefault(x => x.email == email);

            if (user == null)
                return null;
            string link = $"{_Configuration["clientBase"]}resetPassword?e={Common.Encrypt(email)}&d={Common.Encrypt(DateTime.UtcNow.ToString())}";
            if (_EmailService.SendResetPasswordEmail(email, link))
            {
                return user;
            }
            return null;
        }

        public SignUpRequest ResetPassword(string password, string resetLink)
        {
            //split and verify reset link
            var splits = resetLink.Split('/');
            var date = DateTime.Parse(Common.Decrypt(splits.LastOrDefault()));
            Console.WriteLine(date);
            if (date.AddDays(1) <= DateTime.UtcNow)
            {
                throw new Exception("Link has been expired");
            }
            var email = (string)Common.Decrypt(splits.Reverse().Skip(1).FirstOrDefault());
            var user = _db.SignUpRequest.FirstOrDefault(x => x.email == email);
            if (user == null)
                throw new Exception("link has been changed");
            user.password = password;
            _db.SaveChanges();
            return user;
        }


        public SignUpRequest VerifyEmail(string link)
        {
            var splits = link.Split('/');
            var date = DateTime.Parse(Common.Decrypt(splits.Reverse().Skip(1).FirstOrDefault()));
            if (date.AddDays(1) <= DateTime.UtcNow)
            {
                throw new Exception("Link has been expired");
            }
            var email = Common.Decrypt(splits.LastOrDefault());
            var user = _db.SignUpRequest.FirstOrDefault(x => x.email == email);
            if (user == null)
                throw new Exception("link has been changed");
            user.IsEmailVerified = true;
            _db.SaveChanges();
            return user;
        }
    }
}
