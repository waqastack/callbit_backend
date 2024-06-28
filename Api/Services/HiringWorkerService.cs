using Api.Contexts;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PaymentService.Helper;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public class HiringWorkerService : IHiringWorkerService
    {
        DBContextModel db;

        public HiringWorkerService(DBContextModel _db)
        {
            this.db = _db;
        }
        public async Task<InviteResponseRqst> GetInvitedWorker(CallCenterFilters model)
        {
            InviteResponseRqst rsp = new InviteResponseRqst();
            rsp.totalCount = 0;
            rsp.inviteRsp = new List<InviteResponse>();
            try
            {
                bool nameCallCenter = false;
                bool title = false;
                bool tag = false;
                if (!string.IsNullOrEmpty(model.nameCallCenter))
                {
                    nameCallCenter = true;
                }
                if (!string.IsNullOrEmpty(model.title))
                {
                    title = true;
                }
                if (!string.IsNullOrEmpty(model.tag))
                {
                    tag = true;
                }
                List<SignUpRequest> users = new List<SignUpRequest>();
                if (nameCallCenter)
                {
                    users = db.SignUpRequest.Where(x => x.type == 2 && x.username.Contains(model.nameCallCenter)).Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();
                    rsp.totalCount = db.SignUpRequest.Where(x => x.type == 2 && x.username.Contains(model.nameCallCenter)).Count();
                }
                else
                {
                    users = db.SignUpRequest.Where(x => x.type == 2).Skip(model.PageSize * model.PageIndex).Take(model.PageSize).ToList();
                    rsp.totalCount = db.SignUpRequest.Where(x => x.type == 2).Count();
                }
                for (int i = 0; i < users.Count; i++)
                {
                    InviteResponse obj = new InviteResponse();
                    int userid = users[i].id;
                    var userInfo = new profileInformation();
                    if (title && tag)
                    {
                        userInfo = db.profileInformation.Where(x => x.userID == users[i].id && x.pTitle.Contains(model.title) && x.pCompaign.Contains(model.tag)).FirstOrDefault();
                    }
                    else if (title && !tag)
                    {
                        userInfo = db.profileInformation.Where(x => x.userID == users[i].id && x.pTitle.Contains(model.title)).FirstOrDefault();
                    }
                    else if (!title && tag)
                    {
                        userInfo = db.profileInformation.Where(x => x.userID == users[i].id && x.pCompaign.Contains(model.tag)).FirstOrDefault();
                    }
                    else
                    {
                        userInfo = db.profileInformation.Where(x => x.userID == users[i].id).FirstOrDefault();
                    }
                    var userProfile = db.UploadProfilePicture.Where(x => x.userID == users[i].id).FirstOrDefault();
                    var invitationQuery = db.Invitation.Where(x => x.senderID == model.userID &&
                    x.recieverID == users[i].id).FirstOrDefault();

                    obj.coworkerName = userInfo == null || string.IsNullOrEmpty(userInfo.pFullName) || userInfo.pFullName == "Your Name" ? users[i].username : userInfo.pFullName;
                    obj.designation = userInfo == null ? "N/A" : userInfo.pTitle;
                    obj.profileID = userInfo == null ? 0 : Convert.ToInt32(userInfo.profileID);
                    obj.releventSkills = 2;
                    obj.description = userInfo == null ? "N/A" : userInfo.pDescription;
                    obj.userID = users[i].id;
                    obj.perHour = "4";
                    obj.favUserTechn = userInfo == null ? "N/A" : userInfo.pCompaign;
                    obj.invitationStatus = null;
                    obj.senderID = "";
                    obj.recieverID = "";
                    obj.sittingCapacity = userInfo == null ? "N/A" : userInfo.pSittingCapacity.ToString();
                    obj.userImage = userProfile == null || string.IsNullOrEmpty(userProfile.picturePath) ? "../../../assets/images/avatar.jpg" : userProfile.picturePath;
                    if (invitationQuery == null)
                    {

                    }
                    else
                    {
                        obj.invitationStatus = invitationQuery.invitationStatus;
                        obj.senderID = invitationQuery.senderID.ToString();
                        obj.recieverID = invitationQuery.recieverID.ToString();
                    }
                    rsp.inviteRsp.Add(obj);
                }

                return rsp;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<InviteResponse>> GetCallCenter()
        {
            try
            {
                List<InviteResponse> query =
                   (from user in db.SignUpRequest
                    join userInfo in db.profileInformation on user.id equals userInfo.userID
                    join invitied in db.Invitation on user.id equals invitied.recieverID into gj
                    from x in gj.DefaultIfEmpty()
                    where user.type == 2
                    select new InviteResponse
                    {
                        coworkerName = string.IsNullOrEmpty(userInfo.pFullName) || userInfo.pFullName == "Your Name" ? user.username : userInfo.pFullName,
                        designation = userInfo.pTitle,
                        profileID = Convert.ToInt32(userInfo.profileID),
                        releventSkills = 2,
                        description = userInfo.pDescription,
                        userID = user.id,
                        perHour = "4",
                        favUserTechn = userInfo.pCompaign,
                        invitationStatus = x.invitationStatus
                    }).ToList();

                return (List<InviteResponse>)query;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<InviteResponse>> GetProposalWorker(GetProposalWorkerRqst model)
        {
            try
            {
                List<InviteResponse> objRst = new List<InviteResponse>();
                var submitProposals = db.SubmitProposal.Where(x => x.compaignID == model.CampaignID).ToList();
                if (submitProposals.Count > 0)
                {
                    foreach (var objRsp in submitProposals)
                    {
                        InviteResponse obj = new InviteResponse();
                        obj.proposalStatus = objRsp.proposalStatus;
                        obj.compaignID = Convert.ToInt32(objRsp.compaignID);
                        obj.proposalID = Convert.ToInt32(objRsp.proposalID);
                        obj.salesRate = objRsp.salesRate.ToString();
                        obj.numberofSales = objRsp.numberofSales.ToString();
                        obj.clientRecive = objRsp.clientRecive.ToString();
                        obj.coverLetter = string.IsNullOrEmpty(objRsp.coverLetter) ? "" : objRsp.coverLetter.ToString();
                        obj.uploadFile = string.IsNullOrEmpty(objRsp.uploadFile) ? "" : objRsp.uploadFile.ToString();
                        obj.createdDate = objRsp.createdDate.ToString();
                        var user = db.SignUpRequest.Where(x => x.id == objRsp.userID).FirstOrDefault();
                        if (user != null)
                        {
                            obj.userID = user.id;
                            obj.perHour = "4";
                            var userInfo = db.profileInformation.Where(x => x.userID == obj.userID).FirstOrDefault();
                            if (userInfo != null)
                            {
                                obj.coworkerName = string.IsNullOrEmpty(userInfo.pFullName) ||
                                    userInfo.pFullName == "Your Name" ? user.username : userInfo.pFullName;
                                obj.designation = userInfo.pTitle;
                                obj.profileID = Convert.ToInt32(userInfo.profileID);
                                obj.releventSkills = 2;
                                obj.description = string.IsNullOrEmpty(userInfo.pDescription) ? "" : userInfo.pDescription;
                                obj.favUserTechn = userInfo.pCompaign;
                                obj.sittingCapacity = userInfo.pSittingCapacity.ToString();
                            }
                        }
                        objRst.Add(obj);
                    }
                }
                return (List<InviteResponse>)objRst;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<MessageRequest> SendMessage(MessageRequest objReq)
        {
            if (db != null)
            {
                try
                {
                    Messages obj = new Messages();
                    obj.isDeleted = false;
                    obj.createdDate = DateTime.Now;
                    obj.compaignID = objReq.compaignID;
                    obj.messageText = objReq.messageText;
                    obj.proposalID = objReq.proposalID;
                    obj.recieverID = objReq.recieverID;
                    obj.recieverType = objReq.recieverType;
                    obj.senderType = objReq.senderType;
                    obj.senderID = objReq.senderID;
                    db.Messages.Add(obj);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return objReq;
        }
        public async Task<InvitationRequest> SendInvitation(InvitationRequest objReq)
        {
            if (db != null)
            {
                try
                {
                    Invitation obj = new Invitation();
                    obj.isDeleted = false;
                    obj.createdDate = DateTime.Now;
                    obj.compaignID = objReq.compaignID;
                    obj.invitationStatus = objReq.invitationStatus;
                    obj.proposalID = objReq.proposalID;
                    obj.recieverID = objReq.recieverID;
                    obj.recieverType = objReq.recieverType;
                    obj.senderType = objReq.senderType;
                    obj.senderID = objReq.senderID;
                    db.Invitation.Add(obj);
                    await db.SaveChangesAsync();
                    Notifications notifiObj = new Notifications();
                    notifiObj.senderID = objReq.senderID;
                    notifiObj.recieverID = objReq.recieverID;
                    notifiObj.proposalID = objReq.proposalID;
                    notifiObj.compaignID = objReq.compaignID;
                    notifiObj.hiredID = 0;
                    notifiObj.invitaionID = obj.invitationID;
                    notifiObj.contractID = 0;
                    notifiObj.senderType = "Client";
                    notifiObj.recieverType = "Call Center";
                    var userName = db.SignUpRequest.Where(x => x.id == objReq.senderID).FirstOrDefault();
                    notifiObj.msg = userName.username + " send you Invitation request";
                    notifiObj.notificationType = "Invitation request Sended";
                    notifiObj.notifcationCount = "1";
                    notifiObj.isDeleted = false;
                    notifiObj.createdDate = DateTime.Now;
                    db.Notifications.Add(notifiObj);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return objReq;
        }
        public async Task<List<ClientInvitationsRsp>> GetInvitationRequests(int userID)
        {
            try
            {
                List<ClientInvitationsRsp> query =
                   (from user in db.SignUpRequest
                    join invitation in db.Invitation on user.id equals invitation.senderID
                    join compaign in db.Compaign on invitation.compaignID equals compaign.id
                    join userProfile in db.profileInformation on user.id equals userProfile.userID
                    where compaign.isActive == true
                    select new ClientInvitationsRsp
                    {
                        clientName = userProfile.pFullName,
                        compaignName = compaign.name,
                        createdDate = invitation.createdDate,
                        invitationID = Convert.ToInt32(invitation.invitationID),
                        invitationStatus = invitation.invitationStatus,
                        compaignID = Convert.ToInt32(invitation.compaignID),
                        designation = userProfile.pTitle,
                        profileID = Convert.ToInt32(userProfile.profileID),
                        description = userProfile.pDescription,
                        userID = user.id,
                        favUserTechn = userProfile.pCompaign,
                        profileIDProfile = Convert.ToInt32(userProfile.profileID),
                        descriptionProfile = userProfile.pDescription,
                        favUserTechnProfile = userProfile.pCompaign,
                        recieverID = Convert.ToInt32(invitation.recieverID),
                        sittingCapacityProfile = userProfile.pSittingCapacity.ToString()
                    }).ToList();
                query = query.Where(x => x.recieverID == userID).ToList();
                return (List<ClientInvitationsRsp>)query;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<ClientInvitationsRqst> UpdateInvitationStatus(ClientInvitationsRqst objReq)
        {
            if (db != null)
            {
                try
                {
                    Invitation obj = new Invitation();
                    var rst = db.Invitation.Where(x => x.invitationID == objReq.invitationID).SingleOrDefault();
                    if (rst != null)
                    {
                        rst.invitationStatus = objReq.invitationStatus;
                    }
                    await db.SaveChangesAsync();
                    var emailFromRst = db.SignUpRequest.Where(x => x.id == rst.senderID).FirstOrDefault();
                    var emailToRst = db.SignUpRequest.Where(x => x.id == rst.recieverID).FirstOrDefault();
                    UserAllow tblReq = new UserAllow();
                    tblReq.receiver = emailToRst == null ? "" : emailToRst.email;
                    tblReq.sender = emailFromRst == null ? "" : emailFromRst.email;
                    var checkEmails = db.UserAllow.Where(x => (x.receiver == emailToRst.email
                      && x.sender == emailFromRst.email) || (x.sender == emailFromRst.email &&
                      x.receiver == emailToRst.email)).FirstOrDefault();
                    if (checkEmails == null)
                    {
                        db.UserAllow.Add(tblReq);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return objReq;
        }
        public async Task<List<InviteResponse>> HiredCallCenter(HiredRequest objReq)
        {
            try
            {
                hiredCallCenter obj = new hiredCallCenter();
                obj.isDeleted = false;
                obj.createdDate = DateTime.Now;
                obj.compaignID = objReq.compaignID;
                obj.hireStatus = objReq.hireStatus;
                obj.proposalID = objReq.proposalID;
                obj.hiredCallerID = objReq.hiredCallerID;
                obj.hiredCallerType = objReq.hiredCallerType;
                obj.hiredByID = objReq.hiredByID;
                obj.hiredByType = objReq.hiredByType;
                db.hiredCallCenters.Add(obj);
                await db.SaveChangesAsync();

                Notifications notifiObj = new Notifications();
                notifiObj.senderID = objReq.hiredByID;
                notifiObj.recieverID = objReq.hiredCallerID;
                notifiObj.proposalID = objReq.proposalID;
                notifiObj.compaignID = objReq.compaignID;
                notifiObj.hiredID = obj.hiredID;
                notifiObj.invitaionID = obj.hiredID;
                notifiObj.contractID = 0;
                notifiObj.senderType = "Client";
                notifiObj.recieverType = "Call Center";
                var userName = db.SignUpRequest.Where(x => x.id == objReq.hiredByID).FirstOrDefault();
                notifiObj.msg = userName.username + " send you Hired request";
                notifiObj.notificationType = "Hired request Sended";
                notifiObj.notifcationCount = "1";
                notifiObj.isDeleted = false;
                notifiObj.createdDate = DateTime.Now;
                db.Notifications.Add(notifiObj);
                db.SaveChanges();

                List<InviteResponse> query =
              (from user in db.SignUpRequest
               join proposal in db.SubmitProposal on user.id equals proposal.userID
               join userInfo in db.profileInformation on user.id equals userInfo.userID
               where user.type == 2 && user.id != obj.hiredCallerID && proposal.compaignID == obj.compaignID
               select new InviteResponse
               {
                   coworkerName = userInfo.pFullName,
                   designation = userInfo.pTitle,
                   profileID = Convert.ToInt32(userInfo.profileID),
                   completedJobs = Convert.ToInt32(proposal.numberofSales),
                   releventSkills = 2,
                   description = userInfo.pDescription,
                   userID = user.id,
                   compaignID = Convert.ToInt32(proposal.compaignID),
                   proposalID = Convert.ToInt32(proposal.proposalID),
                   perHour = "4",
                   applicantDetail = proposal.coverLetter,
                   favUserTechn = userInfo.pCompaign,
                   proposedBid = Convert.ToInt32(proposal.clientRecive)
               }).ToList();

                return (List<InviteResponse>)query;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<ClientHiredRsp>> GetHiredRequests(int userID, string type)
        {
            try
            {
                List<ClientHiredRsp> query = new List<ClientHiredRsp>();
                if (type == "CallCenter")
                {
                    var rst = db.hiredCallCenters.Where(x => x.hiredCallerID == userID).ToList();
                    query =
                      (from hiredCall in rst
                       join compaign in db.Compaign on hiredCall.compaignID equals compaign.id
                       join user in db.SignUpRequest on hiredCall.hiredByID equals user.id
                       join userProfile in db.profileInformation on user.id equals userProfile.userID
                       where compaign.isActive == true
                       select new ClientHiredRsp
                       {
                           clientName = string.IsNullOrEmpty(userProfile.pFullName) || userProfile.pFullName == "Your Name" ? user.username : userProfile.pFullName,
                           compaignName = compaign.name,
                           createdDate = hiredCall.createdDate,
                           hiredID = Convert.ToInt32(hiredCall.hiredID),
                           hireStatus = hiredCall.hireStatus,
                           compaignID = Convert.ToInt32(hiredCall.compaignID),
                           clientID = Convert.ToInt32(userProfile.userID),
                           email = user.email,

                           profileIDProfile = Convert.ToInt32(userProfile.profileID),
                           descriptionProfile = userProfile.pDescription,
                           favUserTechnProfile = userProfile.pCompaign,
                           sittingCapacityProfile = userProfile.pSittingCapacity.ToString()
                       }).ToList();
                }
                else
                {
                    query =
                      (from user in db.SignUpRequest
                       join hiredCall in db.hiredCallCenters on user.id equals hiredCall.hiredCallerID
                       join compaign in db.Compaign on hiredCall.compaignID equals compaign.id
                       join userProfile in db.profileInformation on user.id equals userProfile.userID
                       join proposal in db.SubmitProposal on user.id equals proposal.userID
                       where hiredCall.hireStatus == "Hired Proposal Accepted" && compaign.isActive == true
                       select new ClientHiredRsp
                       {
                           clientName = string.IsNullOrEmpty(userProfile.pFullName) || userProfile.pFullName == "Your Name" ? user.username : userProfile.pFullName,
                           compaignName = compaign.name,
                           createdDate = hiredCall.createdDate,
                           hiredID = Convert.ToInt32(hiredCall.hiredID),
                           hireStatus = hiredCall.hireStatus,
                           compaignID = Convert.ToInt32(hiredCall.compaignID),
                           designation = userProfile.pTitle,
                           clientID = Convert.ToInt32(userProfile.userID),
                           email = user.email,
                           coworkerNameProfile = string.IsNullOrEmpty(userProfile.pFullName)
                           || userProfile.pFullName == "Your Name" ?
                           user.username : userProfile.pFullName,

                           profileIDProfile = Convert.ToInt32(userProfile.profileID),
                           descriptionProfile = userProfile.pDescription,
                           favUserTechnProfile = userProfile.pCompaign,
                           proposalStatusProfile = proposal.proposalStatus,
                           salesRateProfile = proposal.salesRate.ToString(),
                           numberofSalesProfile = proposal.numberofSales.ToString(),
                           clientReciveProfile = proposal.clientRecive.ToString(),
                           coverLetterProfile = proposal.coverLetter.ToString(),
                           createdDateProfile = proposal.createdDate.ToString(),
                           sittingCapacityProfile = userProfile.pSittingCapacity.ToString()
                       }).ToList();
                    query = query.GroupBy(x => new { x.hiredID }).Select(g => g.First()).ToList();
                }
                return query;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<ClientHiredRqst> UpdateHiredStatus(ClientHiredRqst objReq)
        {
            if (db != null)
            {
                try
                {
                    var rst = db.hiredCallCenters.Where(x => x.hiredID == objReq.hiredID).SingleOrDefault();
                    if (rst != null)
                    {
                        rst.hireStatus = objReq.hireStatus;
                    }
                    await db.SaveChangesAsync();

                    int senderid = Convert.ToInt32(rst.hiredCallerID);
                    Notifications notifiObj = new Notifications();
                    notifiObj.senderID = rst.hiredCallerID;
                    notifiObj.recieverID = rst.hiredByID;
                    notifiObj.proposalID = 0;
                    notifiObj.compaignID = rst.compaignID;
                    notifiObj.hiredID = objReq.hiredID;
                    notifiObj.invitaionID = 0;
                    notifiObj.contractID = 0;
                    notifiObj.senderType = "Call Center";
                    notifiObj.recieverType = "Client";
                    var userName = db.SignUpRequest.Where(x => x.id == senderid).FirstOrDefault();
                    notifiObj.msg = userName.username + " accepted your Hired Request";
                    notifiObj.notificationType = "Hired Request Accepted By Call Center";
                    notifiObj.notifcationCount = "1";
                    notifiObj.isDeleted = false;
                    notifiObj.createdDate = DateTime.Now;
                    db.Notifications.Add(notifiObj);
                    db.SaveChanges();

                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return objReq;
        }
        public async Task<ClientLeadSubRqst> saveLeadSubForm(ClientLeadSubRqst objReq)
        {
            if (db != null)
            {
                try
                {
                    leadSubmisssion objSub = new leadSubmisssion();
                    objSub.compaignID = objReq.compaignID;
                    objSub.requestorUserID = objReq.requestorUserID;
                    objSub.userID = Convert.ToInt32(objReq.userID);
                    objSub.title = objReq.title;
                    objSub.description = objReq.description;
                    objSub.customerName = objReq.customerName;
                    objSub.customerPhnNum = objReq.customerPhnNum;
                    objSub.zipCode = objReq.zipCode;
                    objSub.isDeleted = false;
                    objSub.createdDate = DateTime.Now;
                    db.Add(objSub);
                    db.SaveChanges();
                    long id = objSub.leadSubID;
                    foreach (var qst in objReq.question)
                    {
                        leadSubmisssionQst objSubQst = new leadSubmisssionQst();
                        objSubQst.leadSubID = id;
                        objSubQst.isDeleted = false;
                        objSubQst.createdDate = DateTime.Now;
                        objSubQst.userID = Convert.ToInt32(objReq.userID);
                        objSubQst.compaignID = objReq.compaignID;
                        objSubQst.requestorUserID = objReq.requestorUserID;
                        objSubQst.question = qst.qsts;
                        db.Add(objSubQst);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return objReq;
        }
        public async Task<LeadSubListViewModel> getSubmittedLeads(int userID, string type)
        {

            try
            {
                LeadSubListViewModel obj = new LeadSubListViewModel();
                if (type == "Client")
                {
                    var rst1 = db._leadSubmisssion.Where(x => x.userID == userID).ToList();
                    obj.obj1 = rst1;
                    var rst2 = db._leadSubmisssionQst.Where(x => x.userID == userID).ToList();
                    obj.obj2 = rst2;
                }
                else
                {
                    var rst1 = db._leadSubmisssion.Where(x => x.requestorUserID == userID).ToList();
                    obj.obj1 = rst1;
                    var rst2 = db._leadSubmisssionQst.Where(x => x.requestorUserID == userID).ToList();
                    obj.obj2 = rst2;
                }
                return obj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<RequestFormNotifiSend> NotificationSend(RequestFormNotifiSend objReq)
        {
            if (db != null)
            {
                try
                {
                    EarningNotification objSub = new EarningNotification();
                    objSub.compaignID = objReq.compaignID;
                    objSub.senderID = objReq.senderID;
                    objSub.recieverID = objReq.recieverID;
                    objSub.count = 1;
                    objSub.isDeleted = false;
                    objSub.createdDate = DateTime.Now;
                    db.Add(objSub);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return objReq;
        }
        public async Task<List<EarningNotification>> getEarningNotification(int userID)
        {

            try
            {
                var rst = db._earningNotification.Where(x => x.recieverID == userID && x.count == 1).ToList();
                return rst;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Task<string> updateNotificationEarning(int userID)
        {

            try
            {
                var rst = db._earningNotification.Where(x => x.recieverID == userID && x.count == 1).ToList();
                foreach (var i in rst)
                {
                    i.count = 0;
                    db.SaveChanges();
                }
                return Task.FromResult(string.Empty);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<hiredCallCenter>> getProposalReviewer(int compaignID)
        {

            try
            {
                var rst = db.hiredCallCenters.Where(x => x.compaignID == compaignID).ToList();
                return rst;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<leadSubmisssionQst> SubmitAnswer(List<leadSubmisssionQst> objReq)
        {
            try
            {
                leadSubmisssionQst obj = new leadSubmisssionQst();
                foreach (var item in objReq)
                {
                    var rst = db._leadSubmisssionQst.Where(x => x.leadSubQstID == item.leadSubQstID).ToList();
                    foreach (var i in rst)
                    {
                        i.answers = item.answers;
                        db.SaveChanges();
                    }
                }

                return obj;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CompaignEditRequest> updateCompaignInfo(CompaignEditRequest objReq)
        {
            if (db != null)
            {
                try
                {
                    var rst = db.Compaign.Where(x => x.id == objReq.id).SingleOrDefault();
                    if (rst != null)
                    {
                        rst.payByText = objReq.payByText;
                        rst.payPerText = objReq.payPerText;
                        rst.price = objReq.price.ToString();
                        rst.sale = objReq.sale;
                        rst.totalAmount = objReq.totalAmount;
                        rst.title = objReq.title;
                        rst.name = objReq.title;
                        rst.description = objReq.description;
                        rst.compaignDuration = objReq.compaignDuration;
                        rst.type = objReq.type;
                        rst.payType = objReq.payType;
                    }
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return objReq;
        }
        public async Task<ContractSignReq> saveContractForm(ContractSignReq objReq)
        {
            try
            {
                ContractSign obj = new ContractSign();
                var rst = db.ContractSign.Where(x => x.CallCenterID == objReq.CallCenterID && x.ClientID == objReq.ClientID && x.CompaignID == objReq.CompaignID).FirstOrDefault();
                if (rst == null)
                {
                    if (objReq.ContractStatus == "Contract Request")
                    {
                        obj.CompaignID = objReq.CompaignID;
                        obj.CallCenterID = objReq.CallCenterID;
                        obj.ClientID = objReq.ClientID;
                        obj.ShiftTime = string.IsNullOrEmpty(objReq.ShiftTime) ? "Evening" : objReq.ShiftTime;
                        obj.StartTime = null;
                        obj.EndTime = null;
                        obj.TimeZone = string.IsNullOrEmpty(objReq.TimeZone) ? "America/Los_Angeles" : objReq.TimeZone;
                        obj.ExpectedLeads = "";
                        obj.Days = "";
                        obj.isDeleted = false;
                        obj.createdDate = DateTime.Now;
                        obj.ContractStatus = objReq.ContractStatus;
                        obj.senderType = objReq.senderType;
                        obj.recieverType = objReq.recieverType;
                        obj.NotfiCount = objReq.notificationCount;
                        db.ContractSign.Add(obj);
                        db.SaveChanges();
                    }
                    else
                    {
                        obj.CompaignID = objReq.CompaignID;
                        obj.CallCenterID = objReq.CallCenterID;
                        obj.ClientID = objReq.ClientID;
                        obj.ShiftTime = string.IsNullOrEmpty(objReq.ShiftTime) ? "Evening" : objReq.ShiftTime;
                        obj.StartTime = Convert.ToDateTime(objReq.StartTime);
                        obj.EndTime = Convert.ToDateTime(objReq.EndTime);
                        obj.TimeZone = string.IsNullOrEmpty(objReq.TimeZone) ? "America/Los_Angeles" : objReq.TimeZone;
                        obj.ExpectedLeads = objReq.ExpectedLeads;
                        obj.Days = objReq.Days.ToString();
                        obj.isDeleted = false;
                        obj.createdDate = DateTime.Now;
                        obj.ContractStatus = objReq.ContractStatus;
                        obj.senderType = objReq.senderType;
                        obj.recieverType = objReq.recieverType;
                        obj.NotfiCount = objReq.notificationCount;
                        db.ContractSign.Add(obj);
                        db.SaveChanges();
                    }
                }
                return objReq;
            }
            catch (Exception e)
            {
                return null;
            }
            return objReq;
        }
        public async Task<ContractsClient> GetOnGoingCompaignDDL(ContractGetReq obj)
        {
            try
            {
                ContractsClient ccReq = new ContractsClient();
                List<CompaignCallCenterResp> compainRst = new List<CompaignCallCenterResp>();
                compainRst = (from compaigns in db.Compaign
                              join contractSi
in db.ContractSign on compaigns.id equals contractSi.CompaignID
                              where contractSi.CallCenterID == obj.userID
                              select new CompaignCallCenterResp
                              {
                                  compaignID = contractSi.CompaignID,
                                  compaignName = compaigns.title,
                                  status = contractSi.ContractStatus
                              }).ToList();
                ccReq.CCC = compainRst;
                return ccReq;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<ContractsClient> GetContractLists(ContractGetReq obj)
        {
            try
            {
                ContractsClient ccReq = new ContractsClient();

                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                List<ContractSignResponse> query = new List<ContractSignResponse>();
                query =
                  (from contractSigns in db.ContractSign
                       //join user in db.SignUpRequest on 
                       //contractSigns.CallCenterID equals SqlFunctions. Convert.(user.id)
                       //join compaigns in db.Compaign on 
                       //contractSigns.CallCenterID equals Convert.ToInt64(compaigns.userid)
                   join userProfile in db.profileInformation
                   on contractSigns.ClientID equals userProfile.userID
                   where contractSigns.CallCenterID == obj.userID
                   select new ContractSignResponse
                   {
                       ContractID = contractSigns.ContractID,
                       CompaignID = contractSigns.CompaignID,
                       CallCenterID = contractSigns.CallCenterID,
                       ClientID = contractSigns.ClientID,
                       //CompaignName = db.Compaign.Where(x=>x.use),
                       ClientName = userProfile.pFullName,
                       CallCenterName = db.profileInformation
    .Where(p => p.userID == obj.userID)
    .Select(p => p.pFullName).FirstOrDefault(),
                       ShiftTime = contractSigns.ShiftTime,
                       StartTime = contractSigns.StartTime.ToString(),
                       EndTime = contractSigns.EndTime.ToString(),
                       TimeZone = contractSigns.TimeZone,
                       ExpectedLeads = contractSigns.ExpectedLeads,
                       RemainingLeads = contractSigns.RemainingLeads,
                       Days = contractSigns.Days,
                       ContractStatus = contractSigns.ContractStatus,
                       btnSubmit = DateTime.Compare(Convert.ToDateTime(contractSigns.EndTime), System.DateTime.Now) > 0 ? true : false
                   }).ToList();
                CompaignContrInfoResp objComp = new CompaignContrInfoResp();
                var clientIDRst = db.ContractSign.Where(x => x.CallCenterID == obj.userID &&
                  x.CompaignID == obj.compaignID).FirstOrDefault();
                if (clientIDRst == null)
                {
                    objComp = new CompaignContrInfoResp();
                }
                else
                {
                    var amountRst = db.contractSaleSub.Where(x => x.CompaignID == obj.compaignID).ToList();
                    int remaining = 0;
                    int release = 0;
                    foreach (var rs in amountRst)
                    {
                        release = release + Convert.ToInt32(rs.price);
                    }
                    var profileInfo = db.profileInformation.Where(x => x.userID == clientIDRst.ClientID).FirstOrDefault();
                    objComp.compaignID = obj.compaignID;
                    objComp.clientName = profileInfo.pFullName;
                    var compaignInfo = db.Compaign.Where(x => x.id == obj.compaignID && x.isActive == true).FirstOrDefault();
                    objComp.compaignName = compaignInfo.title;
                    objComp.activeDate = compaignInfo.createdDate.ToString();
                    objComp.escrow = compaignInfo.totalAmount.ToString();
                    objComp.release = release.ToString();
                    remaining = Convert.ToInt32(compaignInfo.totalAmount) - release;
                    objComp.remaining = remaining.ToString();
                    objComp.totalBudget = compaignInfo.totalAmount.ToString();
                    objComp.timeDuration = compaignInfo.compaignDuration.ToString();
                }
                long compaignID = obj.compaignID;
                ccReq._contractSubmittedReponse = (from s in db.contractSaleSub
                                                   where s.CompaignID == compaignID
                                                   select new ContractSubmittedReponse
                                                   {
                                                       SaleSubmittedID = s.SaleSubmittedID,
                                                       ContractID = s.ContractID,
                                                       CompaignID = s.CompaignID,
                                                       CallCenterID = s.CallCenterID,
                                                       ClientID = s.ClientID,
                                                       ExpectedLeads = s.ExpectedLeads,
                                                       createdDate = s.createdDate,
                                                       SaleSUbmittedStatus = s.SaleSUbmittedStatus,
                                                       comment = s.comment

                                                   }).ToList();
                ccReq.CCI = objComp;
                ccReq.CSR = query;

                return ccReq;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<ContractsViewReq> GetOnGoingDDLClient(ContractGetReq obj)
        {
            try
            {
                ContractsViewReq req = new ContractsViewReq();
                var rstContr = db.ContractSign.Where(x => x.ClientID == obj.userID).ToList();
                List<CallCenterClientResp> callCenterClientRst = new List<CallCenterClientResp>();
                foreach (var rst in rstContr)
                {
                    CallCenterClientResp c = new CallCenterClientResp();
                    var rstprofileCall = db.profileInformation.Where(x => x.userID == rst.CallCenterID).FirstOrDefault();
                    var rstprofileClient = db.profileInformation.Where(x => x.userID == rst.ClientID).FirstOrDefault();
                    var rstUsersCall = db.SignUpRequest.Where(x => x.id == rst.CallCenterID).FirstOrDefault();
                    var rstUsersClient = db.SignUpRequest.Where(x => x.id == rst.ClientID).FirstOrDefault();
                    c.callCenterUsrID = rstprofileCall.userID;
                    c.callCenterUsrName = rstprofileCall.pFullName;
                    c.callCenterEmail = rstUsersCall.email;
                    c.compaignID = rst.CompaignID;
                    c.status = rst.ContractStatus;
                    callCenterClientRst.Add(c);
                    CallCenterClientResp c1 = new CallCenterClientResp();
                    c1.callCenterUsrID = rstprofileClient.userID;
                    c1.callCenterUsrName = rstprofileClient.pFullName;
                    c1.callCenterEmail = rstUsersClient.email;
                    c1.compaignID = rst.CompaignID;
                    c1.status = rst.ContractStatus;
                    callCenterClientRst.Add(c1);
                }
                List<CompaignClientResp> compaignClientRst = new List<CompaignClientResp>();
                compaignClientRst = (from compaigns in db.Compaign
                                     join contractSi
       in db.ContractSign on compaigns.id equals contractSi.CompaignID
                                     where contractSi.ClientID == obj.userID
                                     select new CompaignClientResp
                                     {
                                         compaignID = contractSi.CompaignID,
                                         compaignName = compaigns.title,
                                         status = contractSi.ContractStatus
                                     }).ToList();
                req.callCenterClientResp = callCenterClientRst;
                req.compaignClientResp = compaignClientRst;
                return req;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<ContractsViewReq> GetContractClient(ContractGetReq obj)
        {
            try
            {
                ContractsViewReq req = new ContractsViewReq();
                List<ContractClientResponse> query = new List<ContractClientResponse>();
                query =
                  (from contractSigns in db.ContractSign
                       //join user in db.SignUpRequest on 
                       //contractSigns.CallCenterID equals SqlFunctions. Convert.(user.id)
                       //join compaigns in db.Compaign on 
                       //contractSigns.CallCenterID equals Convert.ToInt64(compaigns.userid)
                   join userProfile in db.profileInformation
                   on contractSigns.ClientID equals userProfile.userID
                   where contractSigns.ClientID == obj.userID
                   select new ContractClientResponse
                   {
                       ContractID = contractSigns.ContractID,
                       CompaignID = contractSigns.CompaignID,
                       CallCenterID = contractSigns.CallCenterID,
                       ClientID = contractSigns.ClientID,
                       //CompaignName = db.Compaign.Where(x=>x.use),
                       ClientName = userProfile.pFullName,
                       CallCenterName = db.profileInformation
    .Where(p => p.userID == contractSigns.CallCenterID)
    .Select(p => p.pFullName).FirstOrDefault(),
                       ShiftTime = contractSigns.ShiftTime,
                       StartTime = contractSigns.StartTime.ToString(),
                       EndTime = contractSigns.EndTime.ToString(),
                       TimeZone = contractSigns.TimeZone,
                       ExpectedLeads = contractSigns.ExpectedLeads,
                       RemainingLeads = contractSigns.RemainingLeads,
                       Days = contractSigns.Days,
                       ContractStatus = contractSigns.ContractStatus,
                   }).ToList();

                CompaignContrInfoResp objComp = new CompaignContrInfoResp();
                if (obj.typeTab == "start")
                {
                    var compaignRst = db.Compaign.Where(x => x.userid == obj.userID && x.isActive == true &&
                                     x.id == obj.compaignID).FirstOrDefault();
                    if (compaignRst == null)
                    {
                        objComp = new CompaignContrInfoResp();
                    }
                    else
                    {
                        var profileInfo = db.profileInformation.Where(x => x.userID == compaignRst.userid).FirstOrDefault();
                        var amountRst = db.contractSaleSub.Where(x => x.CompaignID == obj.compaignID).ToList();
                        int remaining = 0;
                        int release = 0;
                        foreach (var rs in amountRst)
                        {
                            release = release + Convert.ToInt32(rs.price);
                        }
                        objComp.compaignID = obj.compaignID;
                        objComp.clientName = profileInfo.pFullName;
                        var compaignInfo = db.Compaign.Where(x => x.id == obj.compaignID && x.isActive == true).FirstOrDefault();
                        objComp.compaignName = compaignInfo.title;
                        objComp.activeDate = compaignInfo.createdDate.ToString();
                        objComp.escrow = compaignInfo.totalAmount.ToString();
                        objComp.release = release.ToString();
                        remaining = Convert.ToInt32(compaignInfo.totalAmount) - release;
                        objComp.remaining = remaining.ToString();
                        objComp.totalBudget = compaignInfo.totalAmount.ToString();
                        objComp.timeDuration = compaignInfo.compaignDuration.ToString();
                    }
                }
                else
                {
                    var contRst = db.ContractSign.Where(x => x.ClientID == obj.userID &&
                    x.CompaignID == obj.compaignID).FirstOrDefault();
                    if (contRst == null)
                    {
                        objComp = new CompaignContrInfoResp();
                    }
                    else
                    {
                        var profileInfo = db.profileInformation.Where(x => x.userID == contRst.CallCenterID).FirstOrDefault();
                        var amountRst = db.contractSaleSub.Where(x => x.CompaignID == obj.compaignID).ToList();
                        int remaining = 0;
                        int release = 0;
                        foreach (var rs in amountRst)
                        {
                            release = release + Convert.ToInt32(rs.price);
                        }
                        objComp.compaignID = obj.compaignID;
                        objComp.clientName = profileInfo.pFullName;
                        var compaignInfo = db.Compaign.Where(x => x.id == obj.compaignID && x.isActive == true).FirstOrDefault();
                        objComp.compaignName = compaignInfo.title;
                        objComp.activeDate = compaignInfo.createdDate.ToString();
                        objComp.escrow = compaignInfo.totalAmount.ToString();
                        objComp.release = release.ToString();
                        remaining = Convert.ToInt32(compaignInfo.totalAmount) - release;
                        objComp.remaining = remaining.ToString();
                        objComp.totalBudget = compaignInfo.totalAmount.ToString();
                        objComp.timeDuration = compaignInfo.compaignDuration.ToString();
                    }
                }
                long compaignID = obj.compaignID;
                req._contractSubmittedReponse = (from s in db.contractSaleSub
                                                 where s.CompaignID == compaignID
                                                 select new ContractSubmittedReponse
                                                 {
                                                     SaleSubmittedID = s.SaleSubmittedID,
                                                     ContractID = s.ContractID,
                                                     CompaignID = s.CompaignID,
                                                     CallCenterID = s.CallCenterID,
                                                     ClientID = s.ClientID,
                                                     ExpectedLeads = s.ExpectedLeads,
                                                     createdDate = s.createdDate,
                                                     SaleSUbmittedStatus = s.SaleSUbmittedStatus,
                                                     comment = s.comment

                                                 }).ToList();
                req.CCI = objComp;
                req.ContractClientRsp = query;
                return req;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<ContractStatusChangeReq> UpdateContractStatus(ContractStatusChangeReq objReq)
        {
            try
            {
                long contractid = Convert.ToInt64(objReq.contractID);
                var rst = db.ContractSign.Where(x => x.ContractID == contractid).FirstOrDefault();
                rst.ContractStatus = objReq.status;
                await db.SaveChangesAsync();

                return new ContractStatusChangeReq();
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Task<string> saveSaleSubmited(ContractSaleSubmittedReq req)
        {
            try
            {
                ContractSaleSubmitted obj = new ContractSaleSubmitted();
                obj.ContractID = req.ContractID;
                obj.CompaignID = req.CompaignID;
                obj.CallCenterID = req.CallCenterID;
                obj.ClientID = req.ClientID;
                obj.ExpectedLeads = req.ExpectedLeads;
                obj.isDeleted = req.isDeleted;
                obj.createdDate = req.createdDate;
                obj.SaleSUbmittedStatus = req.SaleSUbmittedStatus;
                obj.createdDate = DateTime.Now;
                obj.isDeleted = false;
                obj.comment = req.comment;
                obj.AmountSendStatus = "Not Send";
                var rstPrice = db.Compaign.Where(x => x.id == req.CompaignID && x.isActive == true).FirstOrDefault();
                obj.price = Convert.ToInt64(req.ExpectedLeads) * Convert.ToInt64(rstPrice.price);
                db.contractSaleSub.Add(obj);
                db.SaveChanges();
                return Task.FromResult(string.Empty);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Task<string> updateSaleSubmitedStatus(ContractSaleApprovalReq req)
        {
            try
            {
                long abc = Convert.ToInt64(req.saleSubmittedIDSale);
                long bca = Convert.ToInt64(req.contractIDSale);
                var rst = db.contractSaleSub.Where(x => x.SaleSubmittedID == abc).FirstOrDefault();
                var rst1 = db.ContractSign.Where(x => x.ContractID == bca).FirstOrDefault();
                if (req.statusSaleApproval == "Accept")
                {
                    rst.SaleSUbmittedStatus = "Accepted";
                    rst.comment = req.commentsTxt;
                    db.SaveChanges();
                    if (string.IsNullOrEmpty(rst1.RemainingLeads))
                    {
                        int value = Convert.ToInt32(rst1.ExpectedLeads) - Convert.ToInt32(req.saleTotal);
                        rst1.RemainingLeads = value.ToString();
                    }
                    else
                    {
                        int sale = Convert.ToInt32(rst1.RemainingLeads) - Convert.ToInt32(req.saleTotal);
                        rst1.RemainingLeads = sale.ToString();
                    }
                    db.SaveChanges();
                }
                else
                {
                    rst.SaleSUbmittedStatus = "Rejected";
                    rst.comment = req.commentsTxt;
                    db.SaveChanges();
                }
                return Task.FromResult(string.Empty);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<dynamic> PayAsync(PaymentModel payment, string secretKey)
        {
            try
            {
                StripeConfiguration.ApiKey = secretKey;

                var tokenOptions = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = payment.cardNumber,
                        ExpMonth = payment.expiryMonth,
                        ExpYear = payment.expiryYear,
                        Cvc = payment.cvc
                    }
                };

                var serviceToken = new TokenService();
                Token stripeToken = await serviceToken.CreateAsync(tokenOptions);

                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = payment.value * 100,
                    Description = "Microservice Test Payment",
                    ReceiptEmail = payment.userEmail,
                    Currency = "eur",
                    Source = stripeToken.Id // the card
                };

                var chargeService = new ChargeService();
                Charge charge = await chargeService.CreateAsync(chargeOptions);
                if (charge.Paid)
                {
                    payment objTbl = new payment();
                    objTbl.cardNumber = payment.cardNumber;
                    objTbl.expiryMonth = payment.expiryMonth;
                    objTbl.expiryYear = payment.expiryYear;
                    objTbl.cvc = payment.cvc;
                    objTbl.value = payment.value;
                    objTbl.userEmail = payment.userEmail;
                    objTbl.compaignID = payment.compaignID;
                    objTbl.clientID = payment.clientID;
                    objTbl.callCenterID = payment.callCenterID;
                    objTbl.senderType = payment.senderType;
                    objTbl.recieverType = payment.recieverType;
                    db._payment.Add(objTbl);
                    db.SaveChanges();
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<List<InviteResponse>> updateProposalStatus(updateProposalStatusRqst objReq)
        {
            try
            {
                long proposalid = objReq.proposalID;
                var rst = db.SubmitProposal.Where(x => x.proposalID == proposalid).FirstOrDefault();
                rst.proposalStatus = objReq.proposalStatus;
                await db.SaveChangesAsync();

                var emailFromRst = db.SignUpRequest.Where(x => x.id == rst.clientID).FirstOrDefault();
                var emailToRst = db.SignUpRequest.Where(x => x.id == rst.userID).FirstOrDefault();
                UserAllow tblReq = new UserAllow();
                tblReq.receiver = emailToRst == null ? "" : emailToRst.email;
                tblReq.sender = emailFromRst == null ? "" : emailFromRst.email;
                var checkEmails = db.UserAllow.Where(x => (x.receiver == emailToRst.email
                  && x.sender == emailFromRst.email) || (x.sender == emailFromRst.email &&
                  x.receiver == emailToRst.email)).FirstOrDefault();
                if (checkEmails == null)
                {
                    db.UserAllow.Add(tblReq);
                    db.SaveChanges();
                }

                int senderid = Convert.ToInt32(rst.clientID);
                Notifications notifiObj = new Notifications();
                notifiObj.senderID = Convert.ToInt64(rst.clientID);
                notifiObj.recieverID = rst.userID;
                notifiObj.proposalID = objReq.proposalID;
                notifiObj.compaignID = rst.compaignID;
                notifiObj.hiredID = 0;
                notifiObj.invitaionID = 0;
                notifiObj.contractID = 0;
                notifiObj.senderType = "Client";
                notifiObj.recieverType = "Call Center";
                var userName = db.SignUpRequest.Where(x => x.id == senderid).FirstOrDefault();
                notifiObj.msg = userName.username + " accepted your Proposal";
                notifiObj.notificationType = "Proposal Accepted";
                notifiObj.notifcationCount = "1";
                notifiObj.isDeleted = false;
                notifiObj.createdDate = DateTime.Now;
                db.Notifications.Add(notifiObj);
                db.SaveChanges();

                List<InviteResponse> query =
   (from user in db.SignUpRequest
    join proposal in db.SubmitProposal on user.id equals proposal.userID
    join userInfo in db.profileInformation on user.id equals userInfo.userID
    where user.type == 2
    select new InviteResponse
    {
        coworkerName = userInfo.pFullName,
        designation = userInfo.pTitle,
        profileID = Convert.ToInt32(userInfo.profileID),
        completedJobs = Convert.ToInt32(proposal.numberofSales),
        releventSkills = 2,
        description = userInfo.pDescription,
        userID = user.id,
        compaignID = Convert.ToInt32(proposal.compaignID),
        proposalID = Convert.ToInt32(proposal.proposalID),
        perHour = "4",
        applicantDetail = proposal.coverLetter,
        favUserTechn = userInfo.pCompaign,
        proposedBid = Convert.ToInt32(proposal.clientRecive),
        proposalStatus = proposal.proposalStatus
    }).ToList();

                return (List<InviteResponse>)query;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Task<string> CheckPayment(checkPaymentExist req)
        {
            try
            {
                long compaignID = Convert.ToInt64(req.compaignID);
                long clientID = Convert.ToInt64(req.clientID);
                var rstCheck = db.ClientPayCampaign.Where(x => x.clientID == clientID &&
                  x.compaignID == compaignID).ToList();
                if (rstCheck.Count > 0)
                    return Task.FromResult("Already Exist");
                else
                    return Task.FromResult("Not Exist");
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Task<string> StatusContractChange(contractStatusUpd req)
        {
            try
            {
                var rstContr = db.ContractSign.Where(x => x.CompaignID == req.compaignID
                && x.CallCenterID == req.callCenterUsrID).FirstOrDefault();
                rstContr.ContractStatus = req.statusContract;
                db.SaveChanges();
                return Task.FromResult("Updated");
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Task<string> FeedbackContract(feedbackContractRqst req)
        {
            try
            {
                if (req.senderType == "Client")
                {
                    var rstContr = db.FeedbackContract.Where(x => x.contractID == req.contractID
                    && x.senderType == "Client").FirstOrDefault();
                    if (rstContr == null)
                    {
                        FeedbackContract obj = new FeedbackContract();
                        //var contrct = db.ContractSign.Where(x => x.CompaignID == req.CompaignID &&
                        //  x.ClientID == req.ClientID && x.CallCenterID == req.CallCenterID &&
                        //  (x.ContractStatus == "Completed" || x.ContractStatus == "Cancelled")).FirstOrDefault();
                        obj.CompaignID = req.CompaignID;
                        obj.CallCenterID = req.CallCenterID;
                        obj.ClientID = req.ClientID;
                        obj.isDeleted = false;
                        obj.createdDate = DateTime.Now;
                        obj.CommentStatus = "Sended";
                        obj.senderType = req.senderType;
                        obj.recieverType = req.recieverType;
                        obj.contractID = Convert.ToInt32(req.contractID);
                        obj.CommentTxt = req.CommentTxt;
                        db.FeedbackContract.Add(obj);
                        db.SaveChanges();
                    }
                    else
                    {

                    }
                }
                if (req.senderType == "Call Center")
                {
                    var rstContr = db.FeedbackContract.Where(x => x.contractID == req.contractID
                    && x.senderType == "Call Center").FirstOrDefault();
                    if (rstContr == null)
                    {
                        FeedbackContract obj = new FeedbackContract();
                        //var contrct = db.ContractSign.Where(x => x.CompaignID == req.CompaignID &&
                        //  x.ClientID == req.ClientID && x.CallCenterID == req.CallCenterID &&
                        //  (x.ContractStatus == "Completed" || x.ContractStatus == "Cancelled")).FirstOrDefault();
                        obj.CompaignID = req.CompaignID;
                        obj.CallCenterID = req.CallCenterID;
                        obj.ClientID = req.ClientID;
                        obj.isDeleted = false;
                        obj.createdDate = DateTime.Now;
                        obj.CommentStatus = "Sended";
                        obj.senderType = req.senderType;
                        obj.recieverType = req.recieverType;
                        obj.contractID = Convert.ToInt32(req.contractID);
                        obj.CommentTxt = req.CommentTxt;
                        db.FeedbackContract.Add(obj);
                        db.SaveChanges();
                    }
                    else
                    {

                    }
                }
                return Task.FromResult("Inserted");
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public Task<feedbackContractRqstRsp> ShowFeedBack(feedbackContractShowRqst req)
        {
            try
            {
                feedbackContractRqstRsp objRsp = new feedbackContractRqstRsp();
                List<FeedbackContractList> objlst = new List<FeedbackContractList>();
                var rst = db.FeedbackContract.Where(x => x.contractID == req.contractID).ToList();
                if (rst.Count > 0)
                {
                    for (int i = 0; i < rst.Count; i++)
                    {
                        FeedbackContractList objRq = new FeedbackContractList();
                        int id = 0;
                        if (rst[i].senderType == "Call Center")
                        {
                            id = Convert.ToInt32(rst[i].CallCenterID);
                        }
                        if (rst[i].senderType == "Client")
                        {
                            id = Convert.ToInt32(rst[i].ClientID);
                        }
                        var usrInfoLst = db.SignUpRequest.Where(x => x.id == id).FirstOrDefault();
                        var usrPicLst = db.UploadProfilePicture.Where(x => x.userID == id).FirstOrDefault();
                        objRq.feedbackID = rst[i].feedbackID;
                        objRq.CompaignID = rst[i].CompaignID;
                        objRq.CallCenterID = rst[i].CallCenterID;
                        objRq.ClientID = rst[i].ClientID;
                        objRq.isDeleted = rst[i].isDeleted;
                        objRq.createdDate = rst[i].createdDate;
                        objRq.CommentStatus = rst[i].CommentStatus;
                        objRq.senderType = rst[i].senderType;
                        objRq.recieverType = rst[i].recieverType;
                        objRq.contractID = rst[i].contractID;
                        objRq.CommentTxt = rst[i].CommentTxt;
                        objRq.userName = usrInfoLst.username;
                        objRq.userID = id;
                        objRq.userPic = usrPicLst.picturePath;
                        objlst.Add(objRq);
                    }
                    if (req.senderType == "Client")
                    {
                        if (rst.Count == 1)
                        {
                            if (rst[0].senderType == "Client")
                            {
                                objRsp.feedBackStatusFlag = false;
                            }
                            else
                            {
                                objRsp.feedBackStatusFlag = true;
                            }
                        }
                        else
                        {
                            if (rst[1].senderType == "Client")
                            {
                                objRsp.feedBackStatusFlag = false;
                            }
                            else
                            {
                                objRsp.feedBackStatusFlag = true;
                            }
                        }
                    }
                    if (req.senderType == "Call Center")
                    {
                        if (rst.Count == 1)
                        {
                            if (rst[0].senderType == "Call Center")
                            {
                                objRsp.feedBackStatusFlag = false;
                            }
                            else
                            {
                                objRsp.feedBackStatusFlag = true;
                            }
                        }
                        else
                        {
                            if (rst[1].senderType == "Call Center")
                            {
                                objRsp.feedBackStatusFlag = false;
                            }
                            else
                            {
                                objRsp.feedBackStatusFlag = true;
                            }
                        }
                    }
                }
                else
                {
                    objRsp.feedBackStatusFlag = true;
                }
                objRsp.FeedbackContractRsp = objlst;
                return Task.FromResult(objRsp);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<int> GetActiveNotifications(int userID)
        {
            try
            {
                int countNotification = db.Notifications.Where(x => x.recieverID == userID && x.notifcationCount == "1").Count();
                return countNotification;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public async Task<AllNotificationsRqst> GetAllNotifications(int userID)
        {
            try
            {
                AllNotificationsRqst obj = new AllNotificationsRqst();
                var notifications = db.Notifications.Where(f => f.recieverID == userID).ToList();
                notifications.ForEach(a => a.notifcationCount = "0");
                db.SaveChanges();
                int countNotification = db.Notifications.Where(x => x.recieverID == userID && x.notifcationCount == "1").Count();
                obj.activeNotificationCount = countNotification;

                int allNotification = db.Notifications.Where(x => x.recieverID == userID).Count();
                obj.allNotificationCount = allNotification;

                obj.NotificationsResponse = (from user in db.SignUpRequest
                                             join notifi in db.Notifications on user.id equals notifi.senderID
                                             join dp in db.UploadProfilePicture on user.id equals dp.userID
                                             into gj
                                             from x in gj.DefaultIfEmpty()
                                             where notifi.recieverID == userID
                                             select new NotificationsResponse
                                             {
                                                 notificationID = notifi.notificationID,
                                                 senderID = notifi.senderID,
                                                 recieverID = notifi.recieverID,
                                                 proposalID = notifi.proposalID,
                                                 compaignID = notifi.compaignID,
                                                 hiredID = notifi.hiredID,
                                                 invitaionID = notifi.invitaionID,
                                                 contractID = notifi.contractID,
                                                 senderType = notifi.senderType,
                                                 recieverType = notifi.recieverType,
                                                 msg = notifi.msg,
                                                 notificationType = notifi.notificationType,
                                                 notifcationCount = notifi.notifcationCount,
                                                 createdDate = notifi.createdDate.ToString(),
                                                 fullName = user.username.ToString(),
                                                 profilePic = string.IsNullOrEmpty(x.picturePath) ? "../../../assets/images/avatar.jpg" : x.picturePath
                                             }).OrderBy(x => x.createdDate).ToList();
                return obj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> UpdateWizardStep(int userID)
        {
            try
            {
                var updateRst = db.SignUpRequest.Where(x => x.id == userID).FirstOrDefault();
                updateRst.stepWizard = true;
                db.SaveChanges();
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> sendChatNotification(MessageChatNotificationRqst rqst)
        {
            try
            {
                var rstFrom = db.SignUpRequest.Where(x => x.email == rqst.from).FirstOrDefault();
                var rstTo = db.SignUpRequest.Where(x => x.email == rqst.to).FirstOrDefault();
                int senderid = Convert.ToInt32(rstFrom.id);
                Notifications notifiObj = new Notifications();
                notifiObj.senderID = rstFrom.id;
                notifiObj.recieverID = rstTo.id;
                notifiObj.proposalID = 0;
                notifiObj.compaignID = 0;
                notifiObj.hiredID = 0;
                notifiObj.invitaionID = 0;
                notifiObj.contractID = 0;
                if (rstFrom.type == 2)
                {
                    notifiObj.senderType = "Call Center";
                    notifiObj.recieverType = "Client";
                }
                else if (rstFrom.type == 1)
                {
                    notifiObj.senderType = "Client";
                    notifiObj.recieverType = "Call Center";
                }
                var userName = db.SignUpRequest.Where(x => x.id == senderid).FirstOrDefault();
                notifiObj.msg = rqst.message;
                notifiObj.notificationType = "Send New Message";
                notifiObj.notifcationCount = "1";
                notifiObj.isDeleted = false;
                notifiObj.createdDate = DateTime.Now;
                db.Notifications.Add(notifiObj);
                db.SaveChanges();
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<AllNotificationsListRqst> GetAllNotificationsList(int userID)
        {
            try
            {
                AllNotificationsListRqst obj = new AllNotificationsListRqst();
                obj.NotificationsResponse = (from user in db.SignUpRequest
                                             join notifi in db.Notifications on user.id equals notifi.senderID
                                             join dp in db.UploadProfilePicture on user.id equals dp.userID
                                             into gj
                                             from x in gj.DefaultIfEmpty()
                                             where notifi.recieverID == userID
                                             select new NotificationsResponse
                                             {
                                                 notificationID = notifi.notificationID,
                                                 senderID = notifi.senderID,
                                                 recieverID = notifi.recieverID,
                                                 proposalID = notifi.proposalID,
                                                 compaignID = notifi.compaignID,
                                                 hiredID = notifi.hiredID,
                                                 invitaionID = notifi.invitaionID,
                                                 contractID = notifi.contractID,
                                                 senderType = notifi.senderType,
                                                 recieverType = notifi.recieverType,
                                                 msg = notifi.msg,
                                                 notificationType = notifi.notificationType,
                                                 notifcationCount = notifi.notifcationCount,
                                                 createdDate = notifi.createdDate.ToString(),
                                                 fullName = user.username.ToString(),
                                                 profilePic = string.IsNullOrEmpty(x.picturePath) ? "../../../assets/images/avatar.jpg" : x.picturePath
                                             }).OrderBy(x => x.createdDate).ToList();

                obj.NotificationsTypeResponse = (from notifi in db.Notifications
                                                 where notifi.recieverID == userID
                                                 select new NotificationsTypeResponse
                                                 {
                                                     notificationType = notifi.notificationType,
                                                 }).ToList();
                return obj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> starRateInsert(StarRatingRqst rqst)
        {
            try
            {
                StarRating obj = new StarRating();
                var rst = db.StarRating.Where(x => x.RateById == rqst.RateById
                && x.ContractId == rqst.ContractId && x.RateToId == rqst.RateToId).SingleOrDefault();
                if (rst == null)
                {
                    obj.StarRate = rqst.StarRate;
                    obj.Vote = "1";
                    obj.TotalVote = rqst.StarRate;
                    obj.ContractId = rqst.ContractId;
                    obj.RateById = rqst.RateById;
                    obj.RateToId = rqst.RateToId;
                    obj.createdDate = DateTime.Now;
                    obj.isDeleted = false;
                    db.StarRating.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    db.StarRating.Remove(rst);
                    db.SaveChanges();
                    obj.StarRate = rqst.StarRate;
                    obj.Vote = "1";
                    obj.TotalVote = rqst.StarRate;
                    obj.ContractId = rqst.ContractId;
                    obj.RateById = rqst.RateById;
                    obj.RateToId = rqst.RateToId;
                    obj.createdDate = DateTime.Now;
                    obj.isDeleted = false;
                    db.StarRating.Add(obj);
                    db.SaveChanges();
                }
                return "Inserted";
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> starRateGet(StarRateFetchRqst rqst)
        {
            try
            {
                string avgRate = "0";
                if (rqst.Type == "Contract")
                {
                    int star1Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId &&
                    x.ContractId == rqst.ContractId && x.StarRate == "1").Count();
                    int star2Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId
                    && x.ContractId == rqst.ContractId && x.StarRate == "2").Count();
                    int star3Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId
                    && x.ContractId == rqst.ContractId && x.StarRate == "3").Count();
                    int star4Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId
                    && x.ContractId == rqst.ContractId && x.StarRate == "4").Count();
                    int star5Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId
                    && x.ContractId == rqst.ContractId && x.StarRate == "5").Count();
                    double rating = GetRating(star1Count, star2Count, star3Count, star4Count, star5Count);
                    avgRate = rating.ToString();
                }
                else
                {
                    int star1Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId && x.StarRate == "1").Count();
                    int star2Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId && x.StarRate == "2").Count();
                    int star3Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId && x.StarRate == "3").Count();
                    int star4Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId && x.StarRate == "4").Count();
                    int star5Count = db.StarRating.Where(x => x.RateToId == rqst.RateToId && x.StarRate == "5").Count();
                    double rating = GetRating(star1Count, star2Count, star3Count, star4Count, star5Count);
                    avgRate = rating.ToString();
                }
                if (avgRate == "NaN")
                {
                    avgRate = "0";
                }
                return avgRate;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private static double GetRating(int star5Count, int star4Count, int star3Count, int star2Count, int star1Count)
        {
            int star5 = star5Count;
            int star4 = star4Count;
            int star3 = star3Count;
            int star2 = star2Count;
            int star1 = star1Count;

            double rating = (double)(5 * star5 + 4 * star4 + 3 * star3 + 2 * star2 + 1 * star1) / (star1 + star2 + star3 + star4 + star5);

            rating = Math.Round(rating, 1);

            return rating;
        }
        public async Task<string> deleteCompaignInfo(int compaignID)
        {
            try
            {


                var rstDelete = db.Compaign.Where(x => x.id == compaignID).FirstOrDefault();
                if (rstDelete != null)
                {
                    db.Compaign.Remove(rstDelete);
                    db.SaveChanges();
                }

                var rstCompaignQstDelete = db.CampaignQuestion.Where(x => x.Compaignid == compaignID).ToList();
                if (rstCompaignQstDelete.Count > 0)
                {
                    foreach (var deleteObj in rstCompaignQstDelete)
                    {
                        db.CampaignQuestion.Remove(deleteObj);

                        db.SaveChanges();
                    }
                }
                var rstCompaignAnswDelete = db.CampaignAnswers.Where(x => x.Compaignid == compaignID).ToList();
                if (rstCompaignAnswDelete.Count > 0)
                {
                    foreach (var deleteObj in rstCompaignAnswDelete)
                    {
                        db.CampaignAnswers.Remove(deleteObj);

                        db.SaveChanges();
                    }
                }

                var rstProposalDelete = db.SubmitProposal.Where(x => x.compaignID == compaignID).FirstOrDefault();
                if (rstProposalDelete != null)
                {
                    db.SubmitProposal.Remove(rstProposalDelete);
                    db.SaveChanges();
                }

                return "Deleted";
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<KeywordsTag>> SearchKeyword(string keyword)
        {
            try
            {
                List<KeywordsTag> rsp = db.KeywordsTag.Where(x => x.KeywordName.StartsWith(keyword)).ToList();
                return rsp;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> addKeyword(KeywordsRqst keyword)
        {
            try
            {
                var already = db.KeywordsTag.Where(x => x.KeywordName == keyword.KeywordName).FirstOrDefault();
                if (already == null)
                {
                    KeywordsTag kT = new KeywordsTag();
                    kT.KeywordName = keyword.KeywordName;
                    kT.Popular = 1;
                    db.KeywordsTag.Add(kT);
                    db.SaveChanges();
                }
                return "Inserted";
            }
            catch (Exception e)
            {
                return "Not Inserted";
            }
        }
        public async Task<ProposalDetailResp> UserProposals(ProposalDetailReq obj)
        {
            ProposalDetailResp objReq = new ProposalDetailResp();
            try
            {
                var query = db.SubmitProposal.Where(x => x.compaignID == obj.proposalID && x.userID == obj.userID).FirstOrDefault();
                if (query == null)
                {
                    query = db.SubmitProposal.Where(x => x.proposalID == obj.proposalID).FirstOrDefault();
                    if (query == null)
                    {

                    }
                    else
                    {
                        objReq.proposalID = Convert.ToInt32(query.proposalID);
                        objReq.compaignID = Convert.ToInt32(query.compaignID);
                        objReq.userID = Convert.ToInt32(query.userID);
                        objReq.salesRate = Convert.ToInt32(query.salesRate);
                        objReq.numberofSales = Convert.ToInt32(query.numberofSales);
                        objReq.clientRecive = Convert.ToInt32(query.clientRecive);
                        objReq.coverLetter = query.coverLetter;
                        objReq.uploadFile = query.uploadFile;
                        objReq.createdDate = query.createdDate.ToString();
                        objReq.proposalStatus = query.proposalStatus;
                        objReq.clientID = query.clientID.ToString();
                        var compquery = db.Compaign.Where(x => x.id == objReq.compaignID && x.isActive == true).FirstOrDefault();
                        objReq.compaignName = compquery.title;
                        var rstQst = (from qst in db.CampaignQuestion
                                      join ans in db.CampaignAnswers on qst.Id equals ans.QuestionId
                                      where qst.Compaignid == objReq.compaignID
                                      &&
                                      ans.ProposalId == objReq.proposalID
                                      select new ProposalAnswerQst
                                      {
                                          Answer = ans.Answer,
                                          CompaignID = qst.Compaignid,
                                          AnswerID = ans.Id,
                                          Qst = qst.Question,
                                          QstID = qst.Id
                                      }).ToList();
                        objReq.answer = rstQst;
                    }
                }
                else
                {
                    objReq.proposalID = Convert.ToInt32(query.proposalID);
                    objReq.compaignID = Convert.ToInt32(query.compaignID);
                    objReq.userID = Convert.ToInt32(query.userID);
                    objReq.salesRate = Convert.ToInt32(query.salesRate);
                    objReq.numberofSales = Convert.ToInt32(query.numberofSales);
                    objReq.clientRecive = Convert.ToInt32(query.clientRecive);
                    objReq.coverLetter = query.coverLetter;
                    objReq.uploadFile = query.uploadFile;
                    objReq.createdDate = query.createdDate.ToString();
                    objReq.proposalStatus = query.proposalStatus;
                    objReq.clientID = query.clientID.ToString();
                    var compquery = db.Compaign.Where(x => x.id == objReq.compaignID && x.isActive == true).FirstOrDefault();
                    objReq.compaignName = compquery.title;
                    var rstQst = (from qst in db.CampaignQuestion
                                  join ans in db.CampaignAnswers on qst.Id equals ans.QuestionId
                                  where qst.Compaignid == objReq.compaignID
                                  &&
                                  ans.ProposalId == objReq.proposalID
                                  select new ProposalAnswerQst
                                  {
                                      Answer = ans.Answer,
                                      CompaignID = qst.Compaignid,
                                      AnswerID = ans.Id,
                                      Qst = qst.Question,
                                      QstID = qst.Id
                                  }).ToList();
                    objReq.answer = rstQst;
                }
                return objReq;
            }
            catch (Exception e)
            {
                return objReq;
            }
        }
        public async Task<bool> ChangeCampaignStatus(CampaignStatusRqst model)
        {
            try
            {
                bool resultFlag = false;
                var query = db.Compaign.Where(x => x.id == model.campaignID).FirstOrDefault();
                if (query == null)
                {
                    resultFlag = false;
                }
                else
                {
                    if (model.status == "Active")
                    {
                        query.isActive = false;
                    }
                    else
                    {
                        query.isActive = true;
                    }
                    resultFlag = true;
                    db.SaveChanges();
                }
                return resultFlag;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task<dynamic> PayAsyncClient(ClientPayCampaignModelRqst payment, string secretKey)
        {
            try
            {
                string customerId = "";
                var customer = db.customerStripe.Where(x => x.userid == payment.clientID).FirstOrDefault();
                var userDetail = db.SignUpRequest.Where(x => x.id == payment.clientID).FirstOrDefault();
                customerId = customer.customer_stripe_id;
                StripeConfiguration.ApiKey = secretKey;
                var userCard = db.cardStripe.Where(x => x.userid == payment.clientID).FirstOrDefault();
                if (userCard == null)
                {
                    //Create Card Service of customer
                    var optionss = new CardCreateOptions
                    {
                        Source = "tok_visa",
                    };
                    var services = new CardService();
                    var cardServ = services.Create(customerId, optionss);
                    //Create account of customer
                    var optionsAccount = new AccountCreateOptions
                    {
                        Type = "custom",
                        Country = "US",
                        Email = userDetail.email,
                        Capabilities = new AccountCapabilitiesOptions
                        {
                            CardPayments = new AccountCapabilitiesCardPaymentsOptions
                            {
                                Requested = true,
                            },
                            Transfers = new AccountCapabilitiesTransfersOptions
                            {
                                Requested = true,
                            },
                        },
                    };
                    var serviceAccount = new AccountService();
                    var accountDetail = serviceAccount.Create(optionsAccount);

                    cardStripe obj = new cardStripe();
                    obj.token_id = payment.tokenID;
                    obj.customer_stripe_id = customerId;
                    obj.card_id = payment.cardid;
                    obj.userid = Convert.ToInt32(payment.clientID);
                    obj.type = customer.type;
                    obj.account_id = accountDetail.Id;

                    db.cardStripe.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    if (userCard.token_id == payment.tokenID)
                    {

                    }
                }
                StripeConfiguration.ApiKey = secretKey;

                var service = new CardService();
                var options = new CardListOptions
                {
                };
                var cards = service.List(customerId, options);


                var charges = new ChargeService();
                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = payment.amount,
                    Description = "Client payed",
                    Currency = "usd",
                    Customer = userCard.customer_stripe_id,
                    Source = userCard.card_id,
                    ReceiptEmail = payment.userEmail,
                    Metadata = new Dictionary<string, string>()
                    {
                        {"OrderId","111" },
                        {"PostCode","LEE111" },
                    }
                });

                if (charge.Paid)
                {
                    ClientPayCampaign objTbl = new ClientPayCampaign();
                    objTbl.lastDigitNumber = payment.lastDigitNumber.ToString();
                    objTbl.expiryMonth = payment.expiryMonth;
                    objTbl.expiryYear = payment.expiryYear;
                    objTbl.amount = payment.amount;
                    objTbl.cardid = userCard.card_id;
                    objTbl.tokenID = payment.tokenID;
                    objTbl.addressZip = payment.addressZip;
                    objTbl.brand = payment.brand;
                    objTbl.userEmail = payment.userEmail;
                    objTbl.compaignID = payment.compaignID;
                    objTbl.clientID = payment.clientID;
                    objTbl.callCenterID = payment.callCenterID;
                    objTbl.senderType = payment.senderType;
                    objTbl.recieverType = payment.recieverType;
                    db.ClientPayCampaign.Add(objTbl);
                    db.SaveChanges();
                    return "Success";
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<List<Compaign>> SearchCampaignOnChange(string camapignTitle)
        {
            try
            {
                List<Compaign> rsp = db.Compaign.Where(x => x.name.StartsWith(camapignTitle)).ToList();
                return rsp;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}