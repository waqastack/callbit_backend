using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;
using Api.Services;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IDBService _DBService;

        public AdminController(IDBService dbService)
        {
            _DBService = dbService;
        }
        [HttpGet]
        [Route("getAllUsers")]
        public async Task<IActionResult> allUsers()
        {
            var res = await _DBService.GetAllUsers();
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }

        [HttpGet]
        [Route("getSingleUserData")]
        public async Task<IActionResult> GetSingleUser(int id)
        {
            var res = await _DBService.GetSingleUser(id);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }

        [HttpPost]
        [Route("updateData")]
        public async Task<IActionResult> UpdateTeacher([FromBody] SignUpRequest teacher)
        {
            int id = 0;
            var res = await _DBService.UpdateUser(id, teacher);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("getAllCallCentreUsers")]
        public async Task<IActionResult> allCallCentreUsers()
        {
            var res = await _DBService.GetAllCallCentreUsers();
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }

        [HttpGet]
        [Route("getAllClients")]
        public async Task<IActionResult> allClients()
        {
            var res = await _DBService.GetAllClients();
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }


        [HttpGet]
        [Route("getAllCampaigns")]
        public async Task<IActionResult> allCampaigns()
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
        [HttpGet]
        [Route("deleteData")]
        public  bool deleteDataUser(int id)
        {
            var res =  _DBService.deleteData(id);
            return res;
        }
        [HttpGet]
        [Route("GetContractLists")]
        public async Task<IActionResult> GetContractLists()
        {
            var res = await _DBService.GetContractLists();
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("GetAllRecievedAmount")]
        public async Task<IActionResult> GetAllRecievedAmount()
        {
            var res = await _DBService.GetAllRecievedAmount();
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("GetPendingSendAmount")]
        public async Task<IActionResult> GetPendingSendAmount()
        {
            var res = await _DBService.GetPendingSendAmount();
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("TransferAmount")]
        public async Task<IActionResult> TransferAmount(int id)
        {
            var res = await _DBService.TransferAmount(id);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("deleteCompaignInfo")]
        public async Task<IActionResult> deleteCompaignInfo(int compaignID)
        {
            var res = await _DBService.deleteCompaignInfo(compaignID);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
    }
}
