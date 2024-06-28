using Api.Models;
using Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        IDBService _DBService;
        IProfileService _ProfileService;
        AuthService _AuthService;
        private string secretKey;
        public AuthController(IConfiguration configuration, IDBService dbService, IProfileService profileService, AuthService authService)
        {
            _configuration = configuration;
            _DBService = dbService;
            _ProfileService = profileService;
            _AuthService = authService;
            secretKey = _configuration.GetValue<string>("StripeKeys:Secretkey");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest _Request)
        {
            var res = await _DBService.Login(_Request);
            if (res != null)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, _Request.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    username = _Request.Username,
                    isEmailVerified = res.IsEmailVerified,
                    type = res.type,
                    id = res.id,
                    profilePic = _ProfileService.GetPortfolioInfoByUser(res.id),
                    fullName = res.username,
                    stepWizard = res.stepWizard
                });

            }
            return Ok("Invalid credientials");
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("CheckUserNameDuplication")]
        public async Task<IActionResult> CheckUserNameDuplication(string _userName)
        {
            if (_userName != "" && _userName != null)
            {
                var res = await _DBService.CheckUserNameDuplication(_userName);

                if (res == "Yes")
                {
                    return Ok("Yes");
                }
                return Ok("No");
            }
            return NotFound("Please write user name");
        }
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest _Request)
        {
            var res = await _DBService.RegisterUser(_Request, secretKey);

            if (res != null)
            {
                // Saving default profile info after signup 
                await _ProfileService.ProfileInfoOnSignUp(res.id, res.username);
                try
                {
                    _AuthService.SendVerificationEmail(res.email);
                }catch{ }
                return Ok("Register Successfully");
            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("GetAllowUser")]
        public async Task<IActionResult> GetAllowUser([FromBody] UserAllowReq _Request)
        {
            var res = await _DBService.GetUserAllow(_Request);

            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("AddAllowUser")]
        public async Task<IActionResult> AddAllowUser([FromBody] UserAllowReq _Request)
        {
            var res = await _DBService.AddUserAllow(_Request);

            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest _Request)
        {
            var res = await _DBService.ChangePassword(_Request);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok(res);
        }


        [HttpPost]
        [Route("ChangeType")]
        public async Task<IActionResult> ChangeType([FromBody] SignUpRequest _Request)
        {
            var res = await _DBService.ChangeType(_Request);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok(res);
        }

        [HttpGet]
        [Route("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult RequestResetPassword([FromQuery] string email)
        {
            try
            {
                var user = _AuthService.SendResetEmail(email);
                if (user == null)
                    return BadRequest("invalid email address");
                return Ok(true);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordRequestModel model)
        {
            try
            {
                Console.WriteLine(model.ResetLink);
                if (model.Password != model.ConfirmPassword)
                    return BadRequest("Password does not match");
                var user = _AuthService.ResetPassword(model.Password, model.ResetLink);
                if (user == null)
                    return StatusCode(500, "Failed to update password");
                return Ok(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("VerifyEmail")]
        [AllowAnonymous]
        public IActionResult VerifyEmail(string link)
        {
            try
            {
                if (string.IsNullOrEmpty(link?.Trim()))
                    return BadRequest("Invalid link");
                return Ok(_AuthService.VerifyEmail(link));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return StatusCode(500, e.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("CheckEmail")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (email != "" && email != null)
            {
                var res = await _DBService.CheckEmail(email);

                if (res == "Yes")
                {
                    return Ok("Yes");
                }
                return Ok("No");
            }
            return NotFound("Please write Email");
        }

    }
}
