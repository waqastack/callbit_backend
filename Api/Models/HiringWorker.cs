using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
	public class InviteResponseRqst
	{
		public int totalCount { get; set; }
		public List<InviteResponse> inviteRsp { get; set; }
	}
	public class InviteResponse
	{
		public string coworkerName { get; set; }
		public string designation { get; set; }
		public string perHour { get; set; }
		public int completedJobs { get; set; }
		public int releventSkills { get; set; }
		public string description { get; set; }
		public int userID { get; set; }
		public int compaignID { get; set; }
		public int proposalID { get; set; }
		public int profileID { get; set; }
		public string applicantDetail { get; set; }
		public string favUserTechn { get; set; }
		public int proposedBid { get; set; }
		public string proposalStatus { get; set; }
		public string invitationStatus { get; set; }
		public string senderID { get; set; }
		public string recieverID { get; set; }
		public string sittingCapacity { get; set; }
		public string userImage { get; set; }
		public List<CampaignAnswer> Answers { get; set; }
		//Proposal//
		public string salesRate { get; set; }
		public string numberofSales { get; set; }
		public string clientRecive { get; set; }
		public string coverLetter { get; set; }
		public string uploadFile { get; set; }
		public string createdDate { get; set; }
	}
	[Table("Messages")]
	public class Messages
	{
		[Key]
		public long messageID { get; set; }
		public long compaignID { get; set; }
		public long proposalID { get; set; }
		public long senderID { get; set; }
		public long recieverID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public string messageText { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
	}
	public class MessageRequest
	{
		public int compaignID { get; set; }
		public int proposalID { get; set; }
		public int senderID { get; set; }
		public int recieverID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public string messageText { get; set; }
	}
	[Table("Invitation")]
	public class Invitation
	{
		[Key]
		public long invitationID { get; set; }
		public long compaignID { get; set; }
		public long proposalID { get; set; }
		public long senderID { get; set; }
		public long recieverID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public string invitationStatus { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
	}
	public class InvitationRequest
	{
		public int compaignID { get; set; }
		public int proposalID { get; set; }
		public int senderID { get; set; }
		public int recieverID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public string invitationStatus { get; set; }
	}
	public class ClientInvitationsRsp
	{
		public string clientName { get; set; }
		public string compaignName { get; set; }
		public int compaignID { get; set; }
		public int proposalID { get; set; }
		public int invitationID { get; set; }
		public string invitationStatus { get; set; }
		public DateTime? createdDate { get; set; }
		public string designation { get; set; }
		public string description { get; set; }
		public int userID { get; set; }
		public int profileID { get; set; }
		public string applicantDetail { get; set; }
		public string favUserTechn { get; set; }

		public string clientReciveProfile { get; set; }
		public string coverLetterProfile { get; set; }
		public string coworkerNameProfile { get; set; }
		public string createdDateProfile { get; set; }
		public string descriptionProfile { get; set; }
		public string favUserTechnProfile { get; set; }
		public string numberofSalesProfile { get; set; }
		public int profileIDProfile { get; set; }
		public string proposalStatusProfile { get; set; }
		public string recieverIDProfile { get; set; }
		public string salesRateProfile { get; set; }
		public string sittingCapacityProfile { get; set; }
		public int recieverID { get; set; }
	}

	public class ClientInvitationsRqst
	{
		public int invitationID { get; set; }
		public string invitationStatus { get; set; }
	}
	[Table("hiredCallCenter")]
	public class hiredCallCenter
	{
		[Key]
		public long hiredID { get; set; }
		public long compaignID { get; set; }
		public long proposalID { get; set; }
		public long hiredByID { get; set; }
		public long hiredCallerID { get; set; }
		public string hiredByType { get; set; }
		public string hiredCallerType { get; set; }
		public string hireStatus { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
	}
	public class HiredRequest
	{
		public int compaignID { get; set; }
		public int proposalID { get; set; }
		public int hiredByID { get; set; }
		public int hiredCallerID { get; set; }
		public string hiredByType { get; set; }
		public string hiredCallerType { get; set; }
		public string hireStatus { get; set; }
	}
	public class ClientHiredRsp
	{
		public string clientName { get; set; }
		public string compaignName { get; set; }
		public int compaignID { get; set; }
		public int clientID { get; set; }
		public int proposalID { get; set; }
		public int hiredID { get; set; }
		public string hireStatus { get; set; }
		public DateTime? createdDate { get; set; }
		public string designation { get; set; }
		public int proposedBid { get; set; }
		public string description { get; set; }
		public string email { get; set; }
		public string clientReciveProfile { get; set; }
		public string coverLetterProfile { get; set; }
		public string coworkerNameProfile { get; set; }
		public string createdDateProfile { get; set; }
		public string descriptionProfile { get; set; }
		public string favUserTechnProfile { get; set; }
		public string numberofSalesProfile { get; set; }
		public int profileIDProfile { get; set; }
		public string proposalStatusProfile { get; set; }
		public string recieverIDProfile { get; set; }
		public string salesRateProfile { get; set; }
		public string sittingCapacityProfile { get; set; }
	}
	public class ClientHiredRqst
	{
		public int hiredID { get; set; }
		public string hireStatus { get; set; }
	}
	[Table("leadSubmisssion")]
	public class leadSubmisssion
	{
		[Key]
		public long leadSubID { get; set; }
		public long compaignID { get; set; }
		public long userID { get; set; }
		public long requestorUserID { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string customerName { get; set; }
		public string customerPhnNum { get; set; }
		public string zipCode { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
	}
	[Table("leadSubmisssionQst")]
	public class leadSubmisssionQst
	{
		[Key]
		public long leadSubQstID { get; set; }
		public long leadSubID { get; set; }
		public long userID { get; set; }
		public long compaignID { get; set; }
		public long requestorUserID { get; set; }
		public string question { get; set; }
		public string answers { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
	}
	public class ClientLeadSubRqst
	{
		public int userID { get; set; }
		public long requestorUserID { get; set; }
		public long compaignID { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string customerName { get; set; }
		public string customerPhnNum { get; set; }
		public string zipCode { get; set; }
		public QstnsLst[] question { get; set; }
	}
	public class QstnsLst
	{
		public string qsts { get; set; }
	}
	public class LeadSubListViewModel
	{
		public List<leadSubmisssion> obj1 { get; set; }
		public List<leadSubmisssionQst> obj2 { get; set; }
	}
	//public class LeadSubList
	//{
	//    public long leadSubID { get; set; }
	//    public long compaignID { get; set; }
	//    public long userID { get; set; }
	//    public string title { get; set; }
	//    public string description { get; set; }
	//    public string customerName { get; set; }
	//    public string customerPhnNum { get; set; }
	//    public string zipCode { get; set; }
	//    public DateTime? createdDate { get; set; }
	//}
	//public class LeadSubQstList
	//{
	//    public long leadSubQstID { get; set; }
	//    public long leadSubID { get; set; }
	//    public long userID { get; set; }
	//    public string question { get; set; }
	//    public DateTime? createdDate { get; set; }
	//}
	[Table("EarningNotification")]
	public class EarningNotification
	{
		[Key]
		public long earningNotifID { get; set; }
		public long? senderID { get; set; }
		public long? recieverID { get; set; }
		public long? count { get; set; }
		public long? compaignID { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
	}
	public class RequestFormNotifiSend
	{
		public int senderID { get; set; }
		public int recieverID { get; set; }
		public int compaignID { get; set; }
	}
	[Table("payment")]
	public class payment
	{
		[Key]
		public int payment_id { get; set; }
		public string cardNumber { get; set; }
		public int? expiryMonth { get; set; }
		public int? expiryYear { get; set; }
		public string cvc { get; set; }
		public int? value { get; set; }
		public string userEmail { get; set; }
		public long? compaignID { get; set; }
		public long? clientID { get; set; }
		public long? callCenterID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
	}
	public class PaymentModel
	{
		public string cardNumber { get; set; }
		public int? expiryMonth { get; set; }
		public int? expiryYear { get; set; }
		public string cvc { get; set; }
		public int? value { get; set; }
		public string userEmail { get; set; }
		public long? compaignID { get; set; }
		public long? clientID { get; set; }
		public long? callCenterID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
	}
	[Table("ContractSign")]
	public class ContractSign
	{
		[Key]
		public long ContractID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public string ShiftTime { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public string TimeZone { get; set; }
		public string ExpectedLeads { get; set; }
		public string RemainingLeads { get; set; }
		public string Days { get; set; }
		public int NotfiCount { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
		public string ContractStatus { get; set; }
	}
	public class ContractSignReq
	{
		public long ContractID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public string ShiftTime { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public string TimeZone { get; set; }
		public string ExpectedLeads { get; set; }
		public int Days { get; set; }
		public string ContractStatus { get; set; }
		public int notificationCount { get; set; }
		public string recieverType { get; set; }
		public string senderType { get; set; }
	}
	public class ContractsClient
	{
		public List<ContractSignResponse> CSR { get; set; }
		public List<CompaignCallCenterResp> CCC { get; set; }
		public CompaignContrInfoResp CCI { get; set; }
		public List<ContractSubmittedReponse> _contractSubmittedReponse { get; set; }
	}
	public class CompaignContrInfoResp
	{
		public long compaignID { get; set; }
		public string compaignName { get; set; }
		public string clientName { get; set; }
		public string activeDate { get; set; }
		public string escrow { get; set; }
		public string release { get; set; }
		public string remaining { get; set; }
		public string totalBudget { get; set; }
		public string timeDuration { get; set; }

	}
	public class CompaignCallCenterResp
	{
		public long compaignID { get; set; }
		public string compaignName { get; set; }
		public string status { get; set; }
	}

	public class ContractSignResponse
	{
		public long ContractID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public string CompaignName { get; set; }
		public string ClientName { get; set; }
		public string CallCenterName { get; set; }
		public string ShiftTime { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public string TimeZone { get; set; }
		public string ExpectedLeads { get; set; }
		public string RemainingLeads { get; set; }
		public string Days { get; set; }
		public string ContractStatus { get; set; }
		public bool btnSubmit { get; set; }
	}
	public class ContractsViewReq
	{
		public List<CallCenterClientResp> callCenterClientResp { get; set; }
		public List<CompaignClientResp> compaignClientResp { get; set; }
		public List<ContractClientResponse> ContractClientRsp { get; set; }
		public CompaignContrInfoResp CCI { get; set; }
		public List<ContractSubmittedReponse> _contractSubmittedReponse { get; set; }
	}
	public class ContractSubmittedReponse
	{
		public long SaleSubmittedID { get; set; }
		public long ContractID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public string ExpectedLeads { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
		public string SaleSUbmittedStatus { get; set; }
		public string comment { get; set; }
	}
	public class CallCenterClientResp
	{
		public long callCenterUsrID { get; set; }
		public string callCenterUsrName { get; set; }
		public string callCenterEmail { get; set; }
		public long compaignID { get; set; }
		public string status { get; set; }
	}
	public class CompaignClientResp
	{
		public long compaignID { get; set; }
		public string compaignName { get; set; }
		public string status { get; set; }
	}
	public class ContractClientResponse
	{
		public long ContractID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public string CompaignName { get; set; }
		public string ClientName { get; set; }
		public string CallCenterName { get; set; }
		public string ShiftTime { get; set; }
		public string StartTime { get; set; }
		public string EndTime { get; set; }
		public string TimeZone { get; set; }
		public string ExpectedLeads { get; set; }
		public string RemainingLeads { get; set; }
		public string Days { get; set; }
		public string ContractStatus { get; set; }
	}
	public class ContractStatusChangeReq
	{
		public int contractID { get; set; }
		public string status { get; set; }
	}
	public class ContractGetReq
	{
		public int userID { get; set; }
		public int compaignID { get; set; }
		public string typeTab { get; set; }
	}
	[Table("ContractSaleSubmitted")]
	public class ContractSaleSubmitted
	{
		[Key]
		public long SaleSubmittedID { get; set; }
		public long ContractID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public string ExpectedLeads { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
		public string SaleSUbmittedStatus { get; set; }
		public string comment { get; set; }
		public long price { get; set; }
		public string AmountSendStatus { get; set; }
	}
	public class ContractSaleSubmittedReq
	{
		public long ContractID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public string ExpectedLeads { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
		public string SaleSUbmittedStatus { get; set; }
		public string comment { get; set; }
		public long price { get; set; }
	}
	public class ContractSaleApprovalReq
	{
		public int saleSubmittedIDSale { get; set; }
		public int contractIDSale { get; set; }
		public int saleTotal { get; set; }
		public string commentsTxt { get; set; }
		public string statusSaleApproval { get; set; }
	}
	[Table("tbl_proposals_info")]
	public class tbl_proposals_info
	{
		[Key]
		public int purchased_proposals_id { get; set; }
		public int user_id { get; set; }
		public int no_of_purchased_proposals { get; set; }
	}
	public class updateProposalStatusRqst
	{
		public string proposalStatus { get; set; }
		public int proposalID { get; set; }
	}
	public class checkPaymentExist
	{
		public int clientID { get; set; }
		public int compaignID { get; set; }
	}
	public class contractStatusUpd
	{
		public int compaignID { get; set; }
		public int callCenterUsrID { get; set; }
		public string statusContract { get; set; }
	}
	[Table("FeedbackContract")]
	public class FeedbackContract
	{
		[Key]
		public long feedbackID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
		public string CommentStatus { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public int contractID { get; set; }
		public string CommentTxt { get; set; }
	}
	public class feedbackContractRqst
	{
		public int CompaignID { get; set; }
		public int CallCenterID { get; set; }
		public int ClientID { get; set; }
		public string CommentStatus { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public int contractID { get; set; }
		public string CommentTxt { get; set; }
	}
	public class feedbackContractShowRqst
	{
		public int contractID { get; set; }
		public string senderType { get; set; }
	}
	public class feedbackContractRqstRsp
	{
		public bool feedBackStatusFlag { get; set; }
		public List<FeedbackContractList> FeedbackContractRsp { get; set; }
	}
	public class FeedbackContractList
	{
		public long feedbackID { get; set; }
		public long CompaignID { get; set; }
		public long CallCenterID { get; set; }
		public long ClientID { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
		public string CommentStatus { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public int contractID { get; set; }
		public string CommentTxt { get; set; }
		public string? userName { get; set; }
		public int? userID { get; set; }
		public string? userPic { get; set; }
	}
	public class userShortInfo
	{
		public string userName { get; set; }
		public string userPic { get; set; }
	}
	[Table("Notifications")]
	public class Notifications
	{
		[Key]
		public long notificationID { get; set; }
		public long senderID { get; set; }
		public long recieverID { get; set; }
		public long? proposalID { get; set; }
		public long? compaignID { get; set; }
		public long? hiredID { get; set; }
		public long? invitaionID { get; set; }
		public long? contractID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public string msg { get; set; }
		public string notificationType { get; set; }
		public string notifcationCount { get; set; }
		public bool isDeleted { get; set; }
		public DateTime? createdDate { get; set; }
	}
	public class NotificationsRequest
	{
		public long notificationID { get; set; }
		public long senderID { get; set; }
		public long recieverID { get; set; }
		public long? proposalID { get; set; }
		public long? compaignID { get; set; }
		public long? hiredID { get; set; }
		public long? invitaionID { get; set; }
		public long? contractID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public string msg { get; set; }
		public string notificationType { get; set; }
		public string notifcationCount { get; set; }
		public string isDeleted { get; set; }
		public string createdDate { get; set; }
	}
	public class NotificationsResponse
	{
		public long notificationID { get; set; }
		public string fullName { get; set; }
		public string profilePic { get; set; }
		public long senderID { get; set; }
		public long recieverID { get; set; }
		public long? proposalID { get; set; }
		public long? compaignID { get; set; }
		public long? hiredID { get; set; }
		public long? invitaionID { get; set; }
		public long? contractID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
		public string msg { get; set; }
		public string notificationType { get; set; }
		public string notifcationCount { get; set; }
		public string isDeleted { get; set; }
		public string createdDate { get; set; }
	}
	public class AllNotificationsRqst
	{
		public List<NotificationsResponse> NotificationsResponse { get; set; }
		public int activeNotificationCount { get; set; }
		public int allNotificationCount { get; set; }
	}
	public class MessageChatNotificationRqst
	{
		public string from { get; set; }
		public string to { get; set; }
		public string message { get; set; }
	}
	public class NotificationsTypeResponse
	{
		public string notificationType { get; set; }
	}
	public class AllNotificationsListRqst
	{
		public List<NotificationsResponse> NotificationsResponse { get; set; }
		public List<NotificationsTypeResponse> NotificationsTypeResponse { get; set; }
	}
	[Table("StarRating")]
	public class StarRating
	{
		[Key]
		public int RatingId { get; set; }
		public string StarRate { get; set; }
		public string Vote { get; set; }
		public string TotalVote { get; set; }
		public string ContractId { get; set; }
		public string RateById { get; set; }
		public string RateToId { get; set; }
		public DateTime createdDate { get; set; }
		public bool isDeleted { get; set; }
	}
	public class StarRatingRqst
	{
		public string StarRate { get; set; }
		public string ContractId { get; set; }
		public string RateById { get; set; }
		public string RateToId { get; set; }
	}
	public class StarRatingRsp
	{
		public string avgRate { get; set; }
	}
	public class StarRateFetchRqst
	{
		public string Type { get; set; }
		public string ContractId { get; set; }
		public string RateById { get; set; }
		public string RateToId { get; set; }
	}
	[Table("KeywordsTag")]
	public class KeywordsTag
	{
		[Key]
		public int KeywordID { get; set; }
		public string KeywordName { get; set; }
		public int Popular { get; set; }
	}
	public class KeywordsRqst
	{
		public string KeywordName { get; set; }
	}
	public class CallCenterFilters
	{
		public int userID { get; set; }
		public string nameCallCenter { get; set; }
		public string title { get; set; }
		public string tag { get; set; }
		public int PageSize { get; set; }
		public int PageIndex { get; set; }
	}
	public class ProposalDetailReq
	{
		public int userID { get; set; }
		public int proposalID { get; set; }
	}
	public class ProposalDetailResp
	{
		public int proposalID { get; set; }
		public int compaignID { get; set; }
		public int userID { get; set; }
		public int salesRate { get; set; }
		public int numberofSales { get; set; }
		public int clientRecive { get; set; }
		public string coverLetter { get; set; }
		public string uploadFile { get; set; }
		public string createdDate { get; set; }
		public string proposalStatus { get; set; }
		public string clientID { get; set; }
		public string compaignName { get; set; }
		public List<ProposalAnswerQst> answer { get; set; }
	}
	public class ProposalAnswerQst
	{
		public int QstID { get; set; }
		public string Qst { get; set; }
		public string Answer { get; set; }
		public int AnswerID { get; set; }
		public int ProposalID { get; set; }
		public int CompaignID { get; set; }
	}
	public class CampaignStatusRqst
	{
		public int campaignID { get; set; }
		public string status { get; set; }
	}
	public class GetProposalWorkerRqst
	{
		public int CampaignID { get; set; }
	}
	[Table("ClientPayCampaign")]
	public class ClientPayCampaign
	{
		[Key]
		public int payment_campaign_id { get; set; }
		public string lastDigitNumber { get; set; }
		public int? expiryMonth { get; set; }
		public int? expiryYear { get; set; }
		public string cardid { get; set; }
		public string tokenID { get; set; }
		public string addressZip { get; set; }
		public string brand { get; set; }
		public int? amount { get; set; }
		public string userEmail { get; set; }
		public long? compaignID { get; set; }
		public long? clientID { get; set; }
		public long? callCenterID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
	}
	public class ClientPayCampaignModelRqst
	{
		public int lastDigitNumber { get; set; }
		public int? expiryMonth { get; set; }
		public int? expiryYear { get; set; }
		public string cardid { get; set; }
		public string tokenID { get; set; }
		public string addressZip { get; set; }
		public string brand { get; set; }
		public int? amount { get; set; }
		public string userEmail { get; set; }
		public long? compaignID { get; set; }
		public long? clientID { get; set; }
		public long? callCenterID { get; set; }
		public string senderType { get; set; }
		public string recieverType { get; set; }
	}
	public class CampaignRecievedAmountRsp
	{
		public string compaignTitle { get; set; }
		public string client { get; set; }
		public string callCenter { get; set; }
		public string recievedAmount { get; set; }
		public string callBitCharges { get; set; }
		public string callCenterAmount { get; set; }
		public string Brand { get; set; }
		public string clientEmail { get; set; }
	}
	public class CampaignRecievedAmountViewModel
	{
		public List<CampaignRecievedAmountRsp> _campaignRecievedAmountRsp { get; set; }
		public int totalRecord { get; set; }
		public int totalRecievedAmount { get; set; }
		public int totalCallBitCharges { get; set; }
		public int totalCallCenterAmount { get; set; }
	}
	public class SendAmountCallCenterRsp
	{
		public int saleSubmittedID { get; set; }
		public string compaignTitle { get; set; }
		public string clientPicture { get; set; }
		public string client { get; set; }
		public string callCenterPicture { get; set; }
		public string callCenter { get; set; }
		public string amount { get; set; }
		public string amountSendStatus { get; set; }
		public string brand { get; set; }
	}
	public class SendAmountCallCenterViewModel
	{
		public List<SendAmountCallCenterRsp> _sendAmountCallCenterRsp { get; set; }
		public int totalRecord { get; set; }
		public int totalAmount { get; set; }
	}
}
