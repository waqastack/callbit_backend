using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaignController : ControllerBase
    {
        IDBService _DBService;

        public CompaignController(IDBService dBService)
        {
            _DBService = dBService;
        }

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> PostCompaign([FromBody] CompaignRequest _Request)
        {
            //string apikey = "AIzaSyD3ErQVoqvwelwuiHqPzXe8PdTpCG9qNaE";

            //string baseUri = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + _Request.Latitude + "," + _Request.Longitude + "&key=" + apikey;
            //string url = string.Format(baseUri);
            //var json = new WebClient().DownloadString(url);
            //GoogleGeoCode jsonResult = JsonConvert.DeserializeObject<GoogleGeoCode>(json);
            //string add = jsonResult.results[0].formatted_address;
            //var country = add.Split(',').Last();

            //_Request.Location = country;
            var res = await _DBService.PostCompaign(_Request);
            if (res != null)
            {
                return Ok("Compaign Posted Successfully");
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("getAll")]
        public async Task<IActionResult> allCompaign()
        {
            var res = await _DBService.GetAllCompaign();
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }

        [HttpGet]
        [Route("getSingle")]
        public async Task<IActionResult> getSingleCompaign(int id)
        {
            var res = await _DBService.GetSingleCompaign(id);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("SubmitProposal")]
        public async Task<IActionResult> SubmitProposal(ProposalModel _request)
        {
            SubmitProposal proposal = new SubmitProposal();
            List<CampaignAnswer> rst = _request.Answers;
            proposal.compaignID = _request.compaignID;
            proposal.userID = _request.userID;
            proposal.salesRate = _request.salesRate;
            proposal.numberofSales = _request.numberofSales;
            proposal.clientRecive = _request.clientRecive;
            proposal.coverLetter = _request.coverLetter;
            proposal.uploadFile = _request.uploadFile;
            proposal.proposalStatus = _request.proposalStatus;
            proposal.clientID = Convert.ToInt32(_request.clientID);
            var res = await _DBService.SubmitProposal(proposal, rst);
            if (res != null)
            {
                return Ok(new { res });
            }
            else
            {
                return Ok("Something went wrong");
            }
        }

        [HttpPost]
        [Route("GetLoginUserCompaign")]
        public async Task<IActionResult> GetLoginUserCompaign(MyCampaignRqst model)
        {
            var res = await _DBService.GetLoginUserCompaign(model);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpGet]
        [Route("GetProposalDetails")]
        public async Task<IActionResult> GetProposalDetails(int id)
        {
            var res = await _DBService.GetDetailsData(id);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpGet]
        [Route("getProfileInfo")]
        public async Task<IActionResult> getProfileInfo(int id)
        {
            var res = await _DBService.getProfileInfo(id);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpGet]
        [Route("GetFile")]
        //download file api  
        public async Task<FileResult> GetFile(string docFile)
        {
            return File(docFile, "application/pdf", "test.pdf");
        }
        [HttpPost]
        [Route("CheckAlreadyProposal")]
        public async Task<IActionResult> CheckAlreadyProposal(CheckProposalReq model)
        {
            var res = await _DBService.CheckAlreadyProposal(model);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
    }
}
