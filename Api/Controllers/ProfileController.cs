using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        IProfileService _ProfileService;
        private string secretKey;
        private readonly IConfiguration _configuration;
        public ProfileController(IProfileService profileService, IConfiguration configuration)
        {
            _configuration = configuration;
            _ProfileService = profileService;
            secretKey = _configuration.GetValue<string>("StripeKeys:Secretkey");
        }
        [HttpGet]
        [Route("getProfilePicture")]
        public async Task<IActionResult> GetProfilePicture(int id)
        {
            var res = await _ProfileService.GetProfilePicture(id, secretKey);

            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpPost("fileupload")]
        public async Task<IActionResult> FileUpload(MyFileUploadClass abc)  // -> property name must be the same used as formdata key
        {
            PortFolioInfoRequest _request = new PortFolioInfoRequest();
            _request.userID = Convert.ToInt64(abc.usrID);
            _request.profileID = Convert.ToInt64(abc.ProfileID);
            _request.portFolioPath = abc.fileName;
            var res = await _ProfileService.PortfolioInfo(_request);
            if (res != null)
            {
                return Ok(new { abc.fileName });
            }
            return Ok(new { string.Empty });
        }
        [HttpPost("FileUploadMSG")]
        public async Task<IActionResult> FileUploadMSG()  // -> property name must be the same used as formdata key
        {
            string resturnPath = string.Empty;
            var file = Request.Form.Files[0];

            //var userID = Request.Form.;
            var folderName = Path.Combine("Resources", "MSG");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                resturnPath = Path.Combine(folderName, fileName);
                //resturnPath = dbPath + ',' + abc.userID + ',' + abc.filess;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            // do the magic here
            return Ok(new { resturnPath });
        }
        [HttpPost]
        [HttpPost]
        [Route("UploadProfilePicture")]
        public async Task<IActionResult> UploadProfilePicture(ProfilePicRqst obj)
        {

            try
            {

                ProfilePicture _Request = new ProfilePicture();
                _Request.userID = Convert.ToInt64(obj.usrID);
                _Request.picturePath = obj.fileName;
                var res = await _ProfileService.UploadProfilePicture(_Request);
                if (res != null)
                {
                    return Ok(new { obj.fileName });

                    //return Ok("Compaign Posted Successfully");
                }
                return Ok(new { obj.fileName });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            //var res = await _ProfileService.UploadProfileImages(_Request);
            //if (res != null)
            //{

            //    return Ok("Compaign Posted Successfully");

            //}
            //return Ok("Something went wrong");

        }

        [HttpGet]
        [Route("getProfileInfo")]
        public async Task<IActionResult> GetProfileInfo(int id)
        {
            var res = await _ProfileService.GetProfileInfo(id);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("profileInfo")]
        public async Task<IActionResult> ProfileInfo(profileInformation _resquest)
        {
            try
            {
                var res = await _ProfileService.ProfileInfo(_resquest);
                if (res != null)
                {
                    return Ok(new { res });
                }
                return Ok(new { res });

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }

        }
        [HttpGet]
        [Route("getPortfolioInfo")]
        public async Task<IActionResult> GetPortfolioInfo(int id)
        {
            var res = await _ProfileService.GetPortfolioInfo(id);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("portfolioInfo")]
        public async Task<IActionResult> PortfolioInfo(PortFolioInfoRequest _request)
        {

            try
            {
                string resturnPath = string.Empty;
                var file = Request.Form.Files[0];

                //var userID = Request.Form.;
                var folderName = Path.Combine("Resources", "Portfolio");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    //resturnPath = dbPath + ',' + abc.filesd + ',' + abc.filess;
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                var res = await _ProfileService.PortfolioInfo(_request);
                if (res != null)
                {
                    return Ok(res);

                    //return Ok("Compaign Posted Successfully");
                }
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpGet]
        [Route("removePortfolioInfo")]
        public async Task<IActionResult> RemovePortfolioInfo(int portFolioID, int profileID)
        {
            var res = await _ProfileService.RemovePortfolioInfo(portFolioID, profileID);
            if (res)
            {
                return Ok(res);
            }
            return Ok(res);

        }
        [HttpPost]
        [Route("SaveCardInfo")]
        public async Task<IActionResult> SaveCardInfo(cardStripeRqst obj)
        {
            var res = await _ProfileService.SaveCardInfo(obj, secretKey);
            return Ok(res);
        }
        public class MyFileUploadClass
        {
            public string fileName { get; set; }
            public int usrID { get; set; }
            public int ProfileID { get; set; }
            // other properties
        }
    }
}
