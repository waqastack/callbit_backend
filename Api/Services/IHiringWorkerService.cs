using Api.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IHiringWorkerService
    {
        Task<InviteResponseRqst> GetInvitedWorker(CallCenterFilters model);
        Task<List<InviteResponse>> GetCallCenter();
        Task<List<InviteResponse>> GetProposalWorker(GetProposalWorkerRqst obj);
        Task<MessageRequest> SendMessage(MessageRequest obj);
        Task<InvitationRequest> SendInvitation(InvitationRequest obj);
        Task<List<ClientInvitationsRsp>> GetInvitationRequests(int userID);
        Task<ClientInvitationsRqst> UpdateInvitationStatus(ClientInvitationsRqst obj);
        Task<List<InviteResponse>> HiredCallCenter(HiredRequest obj);
        Task<List<ClientHiredRsp>> GetHiredRequests(int userID, string type);
        Task<ClientHiredRqst> UpdateHiredStatus(ClientHiredRqst obj);
        Task<ClientLeadSubRqst> saveLeadSubForm(ClientLeadSubRqst obj);
        Task<LeadSubListViewModel> getSubmittedLeads(int userID, string type);
        Task<RequestFormNotifiSend> NotificationSend(RequestFormNotifiSend obj);
        Task<List<EarningNotification>> getEarningNotification(int userID);
        Task<string> updateNotificationEarning(int userID);
        Task<List<hiredCallCenter>> getProposalReviewer(int compaignID);
        Task<leadSubmisssionQst> SubmitAnswer(List<leadSubmisssionQst> obj);
        Task<CompaignEditRequest> updateCompaignInfo(CompaignEditRequest obj);
        Task<ContractSignReq> saveContractForm(ContractSignReq obj);
        Task<ContractsClient> GetContractLists(ContractGetReq obj);
        Task<ContractsClient> GetOnGoingCompaignDDL(ContractGetReq obj);
        Task<ContractsViewReq> GetContractClient(ContractGetReq obj);
        Task<ContractStatusChangeReq> UpdateContractStatus(ContractStatusChangeReq req);
        Task<string> saveSaleSubmited(ContractSaleSubmittedReq req);
        Task<string> updateSaleSubmitedStatus(ContractSaleApprovalReq req);
        Task<ContractsViewReq> GetOnGoingDDLClient(ContractGetReq obj);
        public Task<dynamic> PayAsync(PaymentModel payment, string secretKey);
        Task<List<InviteResponse>> updateProposalStatus(updateProposalStatusRqst obj);
        Task<string> CheckPayment(checkPaymentExist obj);
        Task<string> StatusContractChange(contractStatusUpd req);
        Task<string> FeedbackContract(feedbackContractRqst req);
        Task<feedbackContractRqstRsp> ShowFeedBack(feedbackContractShowRqst obj);
        Task<int> GetActiveNotifications(int userID);
        Task<AllNotificationsRqst> GetAllNotifications(int userID);
        Task<string> UpdateWizardStep(int userID);
        Task<string> sendChatNotification(MessageChatNotificationRqst rqst);
        Task<AllNotificationsListRqst> GetAllNotificationsList(int userID);
        Task<string> starRateInsert(StarRatingRqst rqst);
        Task<string> starRateGet(StarRateFetchRqst rqst);
        Task<string> deleteCompaignInfo(int compaignID);
        Task<List<KeywordsTag>> SearchKeyword(string keyword);
        Task<string> addKeyword(KeywordsRqst keyword);
        Task<ProposalDetailResp> UserProposals(ProposalDetailReq obj);
        Task<bool> ChangeCampaignStatus(CampaignStatusRqst model);
        public Task<dynamic> PayAsyncClient(ClientPayCampaignModelRqst payment, string secretKey);
        Task<List<Compaign>> SearchCampaignOnChange(string camapignTitle);
    }
}
