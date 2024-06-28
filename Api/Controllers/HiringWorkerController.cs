using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HiringWorkerController : ControllerBase
    {
        IHiringWorkerService _HiringWorkerService;
        private ValidateUser _validateUser;
        private IConfiguration _configuration;
        private string secretKey;
        public HiringWorkerController(IHiringWorkerService hiringWorkerService, IConfiguration configuration)
        {
            _HiringWorkerService = hiringWorkerService;
            _validateUser = new ValidateUser();
            _configuration = configuration;
            secretKey = _configuration.GetValue<string>("StripeKeys:Secretkey");
        }
        [HttpPost]
        [Route("GetInvitedWorker")]
        public async Task<IActionResult> GetInvitedWorker(CallCenterFilters model)
        {
            var res = await _HiringWorkerService.GetInvitedWorker(model);
            if (res != null)
            {

                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("GetCallCenter")]
        public async Task<IActionResult> GetCallCenter()
        {
            var res = await _HiringWorkerService.GetCallCenter();
            if (res != null)
            {

                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("GetProposalWorker")]
        public async Task<IActionResult> GetProposalWorker(GetProposalWorkerRqst obj)
        {
            var res = await _HiringWorkerService.GetProposalWorker(obj);
            if (res != null)
            {

                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("SendMessage")]
        public async Task<IActionResult> SendMessage(MessageRequest obj)
        {
            var res = await _HiringWorkerService.SendMessage(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("SendInvitation")]
        public async Task<IActionResult> SendInvitation(InvitationRequest obj)
        {
            var res = await _HiringWorkerService.SendInvitation(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("GetInvitationRequests")]
        public async Task<IActionResult> GetInvitationRequests(int userID)
        {
            var res = await _HiringWorkerService.GetInvitationRequests(userID);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("UpdateInvitationStatus")]
        public async Task<IActionResult> UpdateInvitationStatus(ClientInvitationsRqst obj)
        {
            var res = await _HiringWorkerService.UpdateInvitationStatus(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("HiredCallCenter")]
        public async Task<IActionResult> HiredCallCenter(HiredRequest obj)
        {
            var res = await _HiringWorkerService.HiredCallCenter(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("GetHiredRequests")]
        public async Task<IActionResult> GetHiredRequests(int userID, string type)
        {
            var res = await _HiringWorkerService.GetHiredRequests(userID, type);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("UpdateHiredStatus")]
        public async Task<IActionResult> UpdateHiredStatus(ClientHiredRqst obj)
        {
            var res = await _HiringWorkerService.UpdateHiredStatus(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("saveLeadSubForm")]
        public async Task<IActionResult> saveLeadSubForm(ClientLeadSubRqst obj)
        {
            var res = await _HiringWorkerService.saveLeadSubForm(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("getSubmittedLeads")]
        public async Task<IActionResult> getSubmittedLeads(int userID, string type)
        {
            var res = await _HiringWorkerService.getSubmittedLeads(userID, type);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("NotificationSend")]
        public async Task<IActionResult> NotificationSend(RequestFormNotifiSend obj)
        {
            var res = await _HiringWorkerService.NotificationSend(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("getEarningNotification")]
        public async Task<IActionResult> getEarningNotification(int userID)
        {
            var res = await _HiringWorkerService.getEarningNotification(userID);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("updateNotificationEarning")]
        public async Task<IActionResult> updateNotificationEarning(int userID)
        {
            var res = await _HiringWorkerService.updateNotificationEarning(userID);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpGet]
        [Route("getProposalReviewer")]
        public async Task<IActionResult> getProposalReviewer(int compaignID)
        {
            var res = await _HiringWorkerService.getProposalReviewer(compaignID);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("SubmitAnswer")]
        public async Task<IActionResult> SubmitAnswer(List<leadSubmisssionQst> obj)
        {
            var res = await _HiringWorkerService.SubmitAnswer(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("updateCompaignInfo")]
        public async Task<IActionResult> updateCompaignInfo(CompaignEditRequest obj)
        {
            var res = await _HiringWorkerService.updateCompaignInfo(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("saveContractForm")]
        public async Task<IActionResult> saveContractForm(ContractSignReq obj)
        {
            var res = await _HiringWorkerService.saveContractForm(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("GetContractLists")]
        public async Task<IActionResult> GetContractLists(ContractGetReq obj)
        {
            var res = await _HiringWorkerService.GetContractLists(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("GetContractClient")]
        public async Task<IActionResult> GetContractClient(ContractGetReq obj)
        {
            var res = await _HiringWorkerService.GetContractClient(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("UpdateContractStatus")]
        public async Task<IActionResult> UpdateContractStatus(ContractStatusChangeReq req)
        {
            var res = await _HiringWorkerService.UpdateContractStatus(req);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("saveSaleSubmited")]
        public async Task<IActionResult> saveSaleSubmited(ContractSaleSubmittedReq req)
        {
            var res = await _HiringWorkerService.saveSaleSubmited(req);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("updateSaleSubmitedStatus")]
        public async Task<IActionResult> updateSaleSubmitedStatus(ContractSaleApprovalReq req)
        {
            var res = await _HiringWorkerService.updateSaleSubmitedStatus(req);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("GetOnGoingCompaignDDL")]
        public async Task<IActionResult> GetOnGoingCompaignDDL(ContractGetReq obj)
        {
            var res = await _HiringWorkerService.GetOnGoingCompaignDDL(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("GetOnGoingDDLClient")]
        public async Task<IActionResult> GetOnGoingDDLClient(ContractGetReq obj)
        {
            var res = await _HiringWorkerService.GetOnGoingDDLClient(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("Payment")]
        public async Task<IActionResult> Payment(PaymentModel payment)
        {
            if (_validateUser.ValidarteParameters(payment))
            {
                string status = await _HiringWorkerService.PayAsync(payment, secretKey);
                if (status != "Success")
                    return BadRequest(status);
                return Ok(status);
            }
            else
                return BadRequest("Wrong parameters!");
        }
        [HttpPost]
        [Route("updateProposalStatus")]
        public async Task<IActionResult> updateProposalStatus(updateProposalStatusRqst obj)
        {
            var res = await _HiringWorkerService.updateProposalStatus(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("CheckPayment")]
        public async Task<IActionResult> CheckPayment(checkPaymentExist obj)
        {
            var res = await _HiringWorkerService.CheckPayment(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("StatusContractChange")]
        public async Task<IActionResult> StatusContractChange(contractStatusUpd obj)
        {
            var res = await _HiringWorkerService.StatusContractChange(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("FeedbackContract")]
        public async Task<IActionResult> FeedbackContract(feedbackContractRqst obj)
        {
            var res = await _HiringWorkerService.FeedbackContract(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("ShowFeedBack")]
        public async Task<IActionResult> ShowFeedBack(feedbackContractShowRqst obj)
        {
            var res = await _HiringWorkerService.ShowFeedBack(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }

        [HttpGet]
        [Route("GetActiveNotifications")]
        public async Task<IActionResult> GetActiveNotifications(int userID)
        {
            var res = await _HiringWorkerService.GetActiveNotifications(userID);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpGet]
        [Route("GetAllNotifications")]
        public async Task<IActionResult> GetAllNotifications(int userID)
        {
            var res = await _HiringWorkerService.GetAllNotifications(userID);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpGet]
        [Route("UpdateWizardStep")]
        public async Task<IActionResult> UpdateWizardStep(int userID)
        {
            var res = await _HiringWorkerService.UpdateWizardStep(userID);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("sendChatNotification")]
        public async Task<IActionResult> sendChatNotification(MessageChatNotificationRqst rqst)
        {
            var res = await _HiringWorkerService.sendChatNotification(rqst);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpGet]
        [Route("GetAllNotificationsList")]
        public async Task<IActionResult> GetAllNotificationsList(int userID)
        {
            var res = await _HiringWorkerService.GetAllNotificationsList(userID);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("starRateInsert")]
        public async Task<IActionResult> starRateInsert(StarRatingRqst rqst)
        {
            var res = await _HiringWorkerService.starRateInsert(rqst);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("starRateGet")]
        public async Task<IActionResult> starRateGet(StarRateFetchRqst rqst)
        {
            var res = await _HiringWorkerService.starRateGet(rqst);
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
            var res = await _HiringWorkerService.deleteCompaignInfo(compaignID);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpGet]
        [Route("SearchKeyword")]
        public async Task<IActionResult> SearchKeyword(string keyword)
        {
            var res = await _HiringWorkerService.SearchKeyword(keyword);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("addKeyword")]
        public async Task<IActionResult> addKeyword(KeywordsRqst obj)
        {
            var res = await _HiringWorkerService.addKeyword(obj);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
        [HttpPost]
        [Route("UserProposals")]
        public async Task<IActionResult> UserProposals(ProposalDetailReq obj)
        {
            var res = await _HiringWorkerService.UserProposals(obj);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("ChangeCampaignStatus")]
        public async Task<IActionResult> ChangeCampaignStatus(CampaignStatusRqst model)
        {
            var res = await _HiringWorkerService.ChangeCampaignStatus(model);
            if (res != null)
            {
                return Ok(res);
            }
            return Ok("Something went wrong");
        }
        [HttpPost]
        [Route("PaymentClient")]
        public async Task<IActionResult> PaymentClient(ClientPayCampaignModelRqst payment)
        {
            string status = await _HiringWorkerService.PayAsyncClient(payment, secretKey);
            if (status != "Success")
                return BadRequest(status);
            return Ok(status);
        }
        [HttpGet]
        [Route("SearchCampaignOnChange")]
        public async Task<IActionResult> SearchCampaignOnChange(string camapignTitle)
        {
            var res = await _HiringWorkerService.SearchCampaignOnChange(camapignTitle);
            if (res != null)
            {

                return Ok(res);

            }
            return Ok("Something went wrong");

        }
    }
}
