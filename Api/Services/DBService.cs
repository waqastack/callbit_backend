using Api.Contexts;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Api.Services
{
    public class DBService : IDBService
    {
        DBContextModel db;

        public DBService(DBContextModel _db)
        {
            this.db = _db;
        }

        public async Task<SignUpRequest> RegisterUser(SignUpRequest s, string secretKey)
        {
            if (db != null)
            {
                try
                {
                    db.SignUpRequest.Add(s);
                    await db.SaveChangesAsync();
                    string desc = s.type == 1 ? "Register User as a Client" : "Register User as a Call Center";
                    StripeConfiguration.ApiKey = secretKey;
                    var options = new CustomerCreateOptions
                    {
                        Description = desc,
                        Email = s.email,
                        Balance = 0,
                        Name = s.username
                    };
                    var service = new CustomerService();
                    Customer customers = service.Create(options);

                    customerStripe cObj = new customerStripe();

                    cObj.customer_stripe_id = customers.Id;
                    cObj.type = s.type;
                    cObj.userid = s.id;
                    db.customerStripe.Add(cObj);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return s;
        }
        public async Task<string> CheckUserNameDuplication(string _userName)
        {
            string response = "No";
            if (db != null)
            {
                try
                {
                    var res = db.SignUpRequest.FirstOrDefault(x => x.username == _userName);
                    //response = res;

                    if (res != null)
                    {
                        response = "Yes";
                        return response;
                    }

                }
                catch (Exception e)
                {
                    return response;
                }
            }

            return response;
        }
        public async Task<SignUpRequest> Login(LoginRequest r)
        {
            List<SignUpRequest> response = new List<SignUpRequest>();
            if (db != null)
            {
                try
                {
                    var res = db.SignUpRequest.Where(x => x.email == r.Username && x.password == r.Password).ToList();
                    if (r.Password == "isGoogle")
                    {
                        res = db.SignUpRequest.Where(x => x.email == r.Username).ToList();
                    }

                    if (res.Count > 0)
                    {
                        return res[0];
                    }

                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
        public async Task<Compaign> PostCompaign(CompaignRequest objReq)
        {
            try
            {
                Compaign obj = new Compaign();
                if (db != null)
                {
                    obj.title = objReq.name;
                    obj.name = objReq.name;
                    obj.type = objReq.type;
                    obj.description = objReq.description;
                    obj.uploadFile = objReq.uploadFile;
                    obj.payType = objReq.payType;
                    obj.describe = objReq.describe;
                    obj.userid = objReq.userID;
                    obj.createdDate = DateTime.Now;
                    obj.isDeleted = false;
                    obj.location = objReq.location;
                    obj.FileNames = objReq.FileNames;
                    obj.inbound = objReq.inbound;
                    obj.outbound = objReq.outbound;
                    //objReq?.Questions.ForEach(x => obj.Questions?.Add(new CampaignQuestion { Question = x }));
                    obj.payPerTextOther = objReq.payPerTextOther;
                    if (objReq.payByText == "Pay By the Sales")
                    {
                        obj.setPrice = objReq.setOwnPriceRbtn;
                        obj.sale = objReq.paySale;
                        obj.price = objReq.payPrice.ToString();
                        obj.totalAmount = objReq.totalAmount;
                        obj.compaignDuration = objReq.compaignDuration;
                    }
                    else
                    {
                        obj.setPrice = false;
                        obj.sale = 0;
                        obj.price = objReq.price;
                        obj.totalAmount = Convert.ToInt64(objReq.price);
                        obj.compaignDuration = "";
                    }
                    obj.payByText = objReq.payByText;
                    obj.payPerText = objReq.payPerText;
                    obj.compaignText = objReq.compaignText;
                    obj.amountDeducted = objReq.amountDeducted;
                    obj.isActive = true;
                    db.Compaign.Add(obj);
                    await db.SaveChangesAsync();

                    for (int i = 0; i < objReq.Questions.Count; i++)
                    {
                        CampaignQuestion objQst = new CampaignQuestion();
                        objQst.Compaignid = obj.id;
                        objQst.Question = objReq.Questions[i];
                        db.CampaignQuestion.Add(objQst);
                        await db.SaveChangesAsync();
                    }
                }
                return obj;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<Compaign>> GetAllCompaign()
        {
            if (db != null)
            {
                try
                {
                    var compaign = db.Compaign.Include(x => x.Questions).Where(x => x.isActive == true).OrderByDescending(x => x.id).ToList();
                    for (int i = 0; i < compaign.Count(); i++)
                    {

                        int Proposal_count = db.SubmitProposal.Where(x => x.compaignID == compaign[i].id).Count();
                        compaign[i].noOfProposals = Proposal_count;

                    }
                    return compaign;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<List<Compaign>> GetSingleCompaign(int id)
        {
            if (db != null)
            {
                try
                {
                    var compaign = db.Compaign.Where(x => x.id == id).ToList();
                    //var compaign = db.Compaign.ToList();
                    return compaign;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
        public async Task<MyCampaignResponse> GetLoginUserCompaign(MyCampaignRqst model)
        {
            MyCampaignResponse obj = new MyCampaignResponse();
            obj.activeCampaignCount = 0;
            obj.inactiveCampaignCount = 0;
            obj.campaign = new List<Compaign>();
            if (db != null)
            {
                try
                {
                    if (model.status == "Active")
                    {
                        obj.campaign = db.Compaign.Include(x => x.Questions).Where(x => x.userid == model.userID
                        && x.isActive == true).ToList();
                    }
                    else
                    {
                        obj.campaign = db.Compaign.Include(x => x.Questions).Where(x => x.userid == model.userID
                        && x.isActive == false).ToList();
                    }
                    //var compaign = db.Compaign.ToList();
                    for (int i = 0; i < obj.campaign.Count(); i++)
                    {

                        int Proposal_count = db.SubmitProposal.Where(x => x.compaignID == obj.campaign[i].id).Count();
                        obj.campaign[i].noOfProposals = Proposal_count;
                        var contractChk = db.ContractSign.Where(x => x.CompaignID == obj.campaign[i].id).FirstOrDefault();
                        if (contractChk == null)
                        {
                            obj.campaign[i].contractStart = false;
                        }
                        else
                        {
                            obj.campaign[i].contractStart = true;
                        }
                    }
                    obj.activeCampaignCount = db.Compaign.Where(x => x.userid == model.userID
                        && x.isActive == true).Count();
                    obj.inactiveCampaignCount = db.Compaign.Where(x => x.userid == model.userID
                     && x.isActive == false).Count();
                    return obj;
                }
                catch (Exception e)
                {
                    return obj;
                }
            }

            return null;
        }
        public bool IsEmailUsed(string email)
        {
            return db.SignUpRequest.Any(x => x.email == email);
        }
        public async Task<SignUpRequest> GetUser(string email)
        {
            if (db != null)
            {
                try
                {
                    return db.SignUpRequest.FirstOrDefault(x => x.email == email);//.ToList();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return null;
        }
        public Task<string> SubmitProposal(SubmitProposal s, List<CampaignAnswer> rst)
        {
            try
            {
                int submitted_Proposal = 0;
                int available_Proposal = 0;
                int purchased_Proposal = 0;
                int remainingProposal = 0;
                var checkCompaign = db.SubmitProposal.Where(x => x.compaignID == s.compaignID
                && x.userID == s.userID).ToList();
                if (checkCompaign.Count > 0)
                {
                    return Task.FromResult("Proposal Already Exist");
                }
                else
                {
                    //Get Submitted Prpoposal
                    submitted_Proposal = db.SubmitProposal.Where(sub_prop => sub_prop.userID
                    == s.userID && sub_prop.createdDate.Year == DateTime.Now.Year && sub_prop.createdDate.Month == DateTime.Now.Month
                    && sub_prop.createdDate.Day == DateTime.Now.Day).
                    Select(x => x.proposalID).Count();

                    available_Proposal = 5 - submitted_Proposal;

                    if (available_Proposal <= 0)
                    {
                        purchased_Proposal = db._tbl_proposals_info.Where(sub_prop => sub_prop.user_id
                        == s.userID).Select(x => x.no_of_purchased_proposals).Count();
                        available_Proposal = available_Proposal + purchased_Proposal;

                        if (purchased_Proposal > 0)
                        {
                            remainingProposal = purchased_Proposal - 1;

                            var _proposals_info = db._tbl_proposals_info.Where(x => x.user_id
                              == s.userID).FirstOrDefault();

                            _proposals_info.no_of_purchased_proposals = remainingProposal;

                            db.SaveChanges();
                        }

                    }
                    if (available_Proposal <= 0)
                    {
                        return Task.FromResult("Please Purchased proposals");
                    }
                    if (available_Proposal > 0)
                    {
                        s.isDeleted = false;
                        s.createdDate = DateTime.Now;
                        var checkCountProp = db.SubmitProposal.Where(x => x.clientID == s.clientID
                          && x.userID == s.userID).ToList();
                        if (checkCountProp.Count > 0)
                        {
                            s.proposalStatus = "Accepted";
                        }
                        else
                        {
                            s.proposalStatus = "Pending";
                        }
                        //if (s.Answers != null)
                        //{
                        //    s.Answers.ForEach(x =>
                        //    {
                        //        x.Question = db.CampaignQuestion.FirstOrDefault(q => q.Id == x.Question.Id);
                        //    });
                        //}
                        db.SubmitProposal.Add(s);
                        db.SaveChanges();
                        for (int j = 0; j < rst.Count; j++)
                        {
                            CampaignAnswer ansObj = new CampaignAnswer();
                            ansObj.Compaignid = Convert.ToInt32(s.compaignID);
                            ansObj.QuestionId = rst[j].QuestionId;
                            ansObj.ProposalId = Convert.ToInt32(s.proposalID);
                            ansObj.Answer = rst[j].Answer;
                            db.CampaignAnswers.Add(ansObj);
                            db.SaveChanges();
                        }
                        //remainingProposal = purchased_Proposal - 1;
                        //var _proposals_info = db._tbl_proposals_info.Where(x => x.user_id
                        //  == s.userID).FirstOrDefault();
                        //_proposals_info.no_of_purchased_proposals = remainingProposal;
                        //db.SaveChanges();
                        return Task.FromResult("Submitted proposal successfully");
                    }
                }
                return Task.FromResult("");
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<string> ChangePassword(ChangePasswordRequest s)
        {
            string returnString = string.Empty;
            if (db != null)
            {
                //SignUpRequest obj = new SignUpRequest();

                try
                {
                    var entity = db.SignUpRequest.FirstOrDefault(item => item.id == s.id && item.email == s.userName && item.password == s.oldPassword);
                    if (entity != null)
                    {
                        entity.password = s.newPassword;
                        db.SignUpRequest.Update(entity);

                        await db.SaveChangesAsync();
                        returnString = "Success";
                    }
                }
                catch (Exception e)
                {
                    return returnString;
                }
            }

            return returnString;
        }
        public async Task<string> ChangeType(SignUpRequest s)
        {
            string returnString = string.Empty;
            if (db != null)
            {
                //SignUpRequest obj = new SignUpRequest();

                try
                {
                    var entity = db.SignUpRequest.FirstOrDefault(item => item.id == s.id);
                    if (entity != null)
                    {
                        entity.type = s.type;
                        db.SignUpRequest.Update(entity);

                        await db.SaveChangesAsync();
                        returnString = "Success";
                    }
                }
                catch (Exception e)
                {
                    return returnString;
                }
            }

            return returnString;
        }
        public async Task<List<UserAllow>> GetUserAllow(UserAllowReq s)
        {

            if (db != null)
            {


                try
                {
                    var res = db.UserAllow.Where(x => x.sender == s.from || x.receiver == s.from).ToList();
                    return res;

                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
        public async Task<string> AddUserAllow(UserAllowReq s)
        {

            if (db != null)
            {


                try
                {

                    UserAllow obj = new UserAllow();
                    obj.sender = s.from;
                    obj.receiver = s.to;
                    var r = db.UserAllow.Where(x => (x.sender == s.from && x.receiver == s.to) || (x.sender == s.to && x.receiver == s.from)).ToList();
                    if (r.Count > 0) { return "Already Added"; }
                    var res = db.UserAllow.Add(obj);
                    await db.SaveChangesAsync();
                    return "Allowed";

                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
        public async Task<List<SignUpRequest>> GetSingleUser(int id)
        {
            if (db != null)
            {
                try
                {
                    var user = db.SignUpRequest.Where(x => x.id == id).ToList();
                    return user;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
        public Task<List<SignUpRequest>> UpdateUser(int id, SignUpRequest teacher)
        {
            var data = db.SignUpRequest.FirstOrDefault(x => x.id == id);
            if (data == null)
            {
                return null;
            }
            data.password = teacher.password;
            try
            {
                db.SignUpRequest.Update(data);
                db.SaveChanges();

            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
        public async Task<List<SignUpRequest>> GetAllUsers()
        {
            if (db != null)
            {
                try
                {
                    var compaign = db.SignUpRequest.ToList();
                    return compaign;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<List<ProposalDetails>> GetDetailsData(int id)
        {

            if (db != null)
            {
                try
                {
                    var listing = new List<ProposalDetails>();

                    var sqlCommand = $@"[dbo].[proc_submitted_available_proposals] {id}";

                    listing = await db.Set<ProposalDetails>().FromSqlRaw(sqlCommand).ToListAsync();
                    return listing;




                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
        public async Task<List<SignUpRequest>> GetAllCallCentreUsers()
        {
            if (db != null)
            {
                try
                {
                    var compaign = db.SignUpRequest.Where(x => x.type == 2).ToList();
                    return compaign;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<List<SignUpRequest>> GetAllClients()
        {
            if (db != null)
            {
                try
                {
                    var compaign = db.SignUpRequest.Where(x => x.type == 1).ToList();
                    return compaign;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
        public bool deleteData(int id)
        {
            var User = db.SignUpRequest.Find(id);
            if (User == null)
            {
                return false;
            }
            try
            {
                db.Remove(User);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<string> CheckEmail(string email)
        {
            string response = "No";
            if (db != null)
            {
                try
                {
                    var res = db.SignUpRequest.FirstOrDefault(x => x.email == email);
                    //response = res;

                    if (res != null)
                    {
                        response = "Yes";
                        return response;
                    }

                }
                catch (Exception e)
                {
                    return response;
                }
            }

            return response;
        }
        public async Task<List<ProfileInfoPopup>> getProfileInfo(int id)
        {
            try
            {
                List<ProfileInfoPopup> query = (from user in db.SignUpRequest
                                                join userProfile in db.profileInformation on user.id equals userProfile.userID
                                                where user.id == id
                                                select new ProfileInfoPopup
                                                {
                                                    userID = id,
                                                    profileID = Convert.ToInt32(userProfile.profileID),
                                                    description = userProfile.pDescription,
                                                    designation = userProfile.pTitle,
                                                    favUserTechn = userProfile.pCompaign,
                                                    sittingCapacity = userProfile.pSittingCapacity,
                                                    userName = string.IsNullOrEmpty(userProfile.pFullName) || userProfile.pFullName == "Your Name" ? user.username : userProfile.pFullName
                                                }).ToList();
                return query;
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }
        public async Task<ContractsClient> GetContractLists()
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
                   // where contractSigns.CallCenterID == obj.userID
                   select new ContractSignResponse
                   {
                       ContractID = contractSigns.ContractID,
                       CompaignID = contractSigns.CompaignID,
                       CallCenterID = contractSigns.CallCenterID,
                       ClientID = contractSigns.ClientID,
                       //CompaignName = db.Compaign.Where(x=>x.use),
                       ClientName = userProfile.pFullName,
                       CallCenterName = db.profileInformation
    //.Where(p => p.userID == obj.userID)
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
                var clientIDRst = db.ContractSign.FirstOrDefault();
                //var clientIDRst = db.ContractSign.Where(x => x.CallCenterID == obj.userID &&
                //  x.CompaignID == obj.compaignID).FirstOrDefault();
                if (clientIDRst == null)
                {
                    objComp = new CompaignContrInfoResp();
                }
                else
                {
                    var profileInfo = db.profileInformation.Where(x => x.userID == clientIDRst.ClientID).FirstOrDefault();
                    var compaignInfo = db.Compaign.FirstOrDefault();
                    objComp.compaignID = compaignInfo.id;
                    objComp.clientName = profileInfo.pFullName;
                    // var compaignInfo = db.Compaign.FirstOrDefault();
                    // var compaignInfo = db.Compaign.Where(x => x.id == obj.compaignID).FirstOrDefault();
                    objComp.compaignName = compaignInfo.title;
                    objComp.activeDate = compaignInfo.createdDate.ToString();
                    objComp.escrow = compaignInfo.totalAmount.ToString();
                    objComp.release = compaignInfo.totalAmount.ToString();
                    objComp.remaining = compaignInfo.totalAmount.ToString();
                    objComp.totalBudget = compaignInfo.totalAmount.ToString();
                    objComp.timeDuration = compaignInfo.compaignDuration.ToString();
                }
                // long compaignID = obj.compaignID;
                ccReq._contractSubmittedReponse = (from s in db.contractSaleSub
                                                       //where s.CompaignID == compaignID
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
        public async Task<string> CheckAlreadyProposal(CheckProposalReq model)
        {
            if (db != null)
            {
                try
                {
                    string result = "";
                    var checkProposal = db.SubmitProposal.Where(x => x.userID == model.UserID && x.compaignID == model.CompaignID).FirstOrDefault();
                    if (checkProposal == null)
                    {
                        result = "Not Exist";
                    }
                    else
                    {
                        result = "Already Exist";
                    }
                    return result;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return null;
        }
        public async Task<CampaignRecievedAmountViewModel> GetAllRecievedAmount()
        {
            CampaignRecievedAmountViewModel obj = new CampaignRecievedAmountViewModel();
            obj._campaignRecievedAmountRsp = new List<CampaignRecievedAmountRsp>();
            obj.totalRecord = 0;
            obj.totalRecievedAmount = 0;
            obj.totalCallBitCharges = 0;
            obj.totalCallCenterAmount = 0;
            try
            {
                var clientPayCampaign = db.ClientPayCampaign.ToList();
                int totalRecievedAmount = 0;
                int totalCallBitCharges = 0;
                int totalCallCenterAmount = 0;
                for (int i = 0; i < clientPayCampaign.Count; i++)
                {
                    CampaignRecievedAmountRsp model = new CampaignRecievedAmountRsp();
                    var campaign = db.Compaign.Where(x => x.id == clientPayCampaign[i].compaignID).FirstOrDefault();
                    var client = db.SignUpRequest.Where(x => x.id == clientPayCampaign[i].clientID).FirstOrDefault();
                    var callcenter = db.SignUpRequest.Where(x => x.id == clientPayCampaign[i].callCenterID).FirstOrDefault();

                    model.compaignTitle = campaign.title;
                    model.client = client.username;
                    model.callCenter = callcenter.username;
                    model.recievedAmount = clientPayCampaign[i].amount.ToString();
                    string recievedAmount = clientPayCampaign[i].amount.ToString();
                    int amount = Convert.ToInt32(recievedAmount);
                    int callBitCharges = (amount * 3) / 100;
                    model.callBitCharges = callBitCharges.ToString();
                    int callCenterAmount = amount - callBitCharges;
                    model.callCenterAmount = callCenterAmount.ToString();
                    model.Brand = clientPayCampaign[i].brand;
                    model.clientEmail = clientPayCampaign[i].userEmail;

                    totalRecievedAmount = totalRecievedAmount + amount;
                    totalCallBitCharges = totalCallBitCharges + callBitCharges;
                    totalCallCenterAmount = totalCallCenterAmount + callCenterAmount;
                    obj._campaignRecievedAmountRsp.Add(model);
                }
                obj.totalRecord = clientPayCampaign.Count();
                obj.totalRecievedAmount = totalRecievedAmount;
                obj.totalCallBitCharges = totalCallBitCharges;
                obj.totalCallCenterAmount = totalCallCenterAmount;
                return obj;
            }
            catch (Exception e)
            {
                return obj;
            }
        }
        public async Task<SendAmountCallCenterViewModel> GetPendingSendAmount()
        {
            SendAmountCallCenterViewModel obj = new SendAmountCallCenterViewModel();
            obj._sendAmountCallCenterRsp = new List<SendAmountCallCenterRsp>();
            obj.totalRecord = 0;
            obj.totalAmount = 0;
            try
            {
                var ContractSaleSubmitted = db.contractSaleSub.Where(x => x.SaleSUbmittedStatus == "Accepted");
                foreach (var rst in ContractSaleSubmitted)
                {
                    SendAmountCallCenterRsp model = new SendAmountCallCenterRsp();
                    var campaign = db.Compaign.Where(x => x.id == rst.CompaignID).FirstOrDefault();
                    var client = db.SignUpRequest.Where(x => x.id == rst.ClientID).FirstOrDefault();
                    var callcenter = db.SignUpRequest.Where(x => x.id == rst.CallCenterID).FirstOrDefault();
                    var callcenterPic = db.UploadProfilePicture.Where(x => x.userID == rst.CallCenterID).FirstOrDefault();
                    var clientPic = db.UploadProfilePicture.Where(x => x.userID == rst.ClientID).FirstOrDefault();
                    string callCenterImg = "../../../assets/images/icons/clients.svg";
                    string clientImg = "../../../assets/images/icons/clients.svg";
                    if (callcenterPic != null)
                    {
                        callCenterImg = callcenterPic.picturePath;
                    }
                    if (clientPic != null)
                    {
                        clientImg = clientPic.picturePath;
                    }
                    model.saleSubmittedID = Convert.ToInt32(rst.SaleSubmittedID);
                    model.compaignTitle = campaign.title;
                    model.clientPicture = clientImg;
                    model.client = client.username;
                    model.callCenterPicture = callCenterImg;
                    model.callCenter = callcenter.username;
                    model.amount = rst.price.ToString();
                    model.amountSendStatus = rst.AmountSendStatus;
                    obj.totalRecord = obj.totalRecord + Convert.ToInt32(rst.price);
                    obj._sendAmountCallCenterRsp.Add(model);
                }
                obj.totalRecord = ContractSaleSubmitted.Count();
                return obj;
            }
            catch (Exception e)
            {
                return obj;
            }
        }
        public async Task<string> TransferAmount(int id)
        {
            try
            {
                var rst = db.contractSaleSub.Where(x => x.SaleSubmittedID == id).FirstOrDefault();
                rst.AmountSendStatus = "Send";
                db.SaveChanges();
                return "Transfer";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
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
    }
}