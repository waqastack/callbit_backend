using Api.Contexts;
using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe;

namespace Api.Services
{
    public class ProfileService : IProfileService
    {
        DBContextModel db;
        public ProfileService(DBContextModel _db)
        {
            this.db = _db;
        }

        public async Task<string> GetProfilePicture(int id, string secretKey)

        {
            string picturePath = string.Empty;
            if (db != null)
            {
                try
                {
                    var response = db.UploadProfilePicture.Where(item => item.userID == id).FirstOrDefault();
                    if (response != null)
                    {
                        if (response != null)
                        {
                            picturePath = response.picturePath;
                        }
                    }

                    return picturePath;
                }
                catch (Exception e)
                {
                    return picturePath;
                }
            }

            return picturePath;
        }
        public async Task<ProfilePicture> UploadProfilePicture(ProfilePicture res)
        {
            if (db != null)
            {
                try
                {
                    var rst = db.UploadProfilePicture.Where(x => x.userID == res.userID).FirstOrDefault();
                    if (rst == null)
                    {
                        res.isDeleted = false;
                        res.createdDate = DateTime.Now;

                        db.UploadProfilePicture.Add(res);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        rst.picturePath = res.picturePath;
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return res;
        }
        public async Task<List<profileInformation>> GetProfileInfo(int id)
        {
            //string picturePath = string.Empty;
            List<profileInformation> response = new List<profileInformation>();
            if (db != null)
            {
                try
                {
                    var result = db.profileInformation.Where(item => item.userID == id && item.isDeleted == false).ToList();
                    if (result.Count > 0)
                    {
                        response = result;
                    }
                    else if (result.Count == 0)
                    {
                        await ProfileInfoOnSignUp(id,"UserName");
                        var res = db.profileInformation.Where(item => item.userID == id && item.isDeleted == false).ToList();
                        response = res;
                    }
                    return response;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                return response;
            }

        }
        public async Task<ProfileInfoResponse> ProfileInfo(profileInformation _request)
        {
            ProfileInfoResponse response = new ProfileInfoResponse();
            if (db != null)
            {
                try
                {

                    var entity = db.profileInformation.FirstOrDefault(item => item.profileID == _request.profileID && item.userID == _request.userID);
                    if (entity != null)
                    {
                        entity.pFullName = _request.pFullName;
                        entity.pTitle = _request.pTitle;
                        entity.pDescription = _request.pDescription;
                        entity.pCompaign = _request.pCompaign;
                        entity.pSittingCapacity = _request.pSittingCapacity;
                        entity.modifiedDate = _request.modifiedDate;
                        db.profileInformation.Update(entity);

                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        _request.isDeleted = false;
                        _request.createdDate = DateTime.Now;

                        db.profileInformation.Add(_request);
                        await db.SaveChangesAsync();
                    }

                    response.profileID = entity.profileID;
                    response.userID = entity.profileID;
                    response.pFullName = entity.pFullName;
                    response.pTitle = entity.pTitle;
                    response.pDescription = entity.pDescription;
                    response.pCompaign = entity.pCompaign;
                    response.pSittingCapacity = entity.pSittingCapacity;


                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return response;
        }
        public async Task<bool> ProfileInfoOnSignUp(int _userID, string _userName)
        {
            if (db != null)
            {
                profileInformation _request = new profileInformation();
                try
                {
                    // _request.profileID   ---  Auto generated
                    _request.userID = _userID;
                    _request.pFullName = _userName;
                    _request.pTitle = "Your Title";
                    _request.pDescription = "Your Description";
                    _request.pCompaign = "";
                    _request.pSittingCapacity = "";
                    _request.isDeleted = false;
                    _request.createdDate = DateTime.Now;
                    _request.modifiedDate = DateTime.Now;

                    db.profileInformation.Add(_request);
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return true;
        }
        public async Task<List<PortFolioInfoResponse>> PortfolioInfo(PortFolioInfoRequest _request)
        {
            List<PortFolioInfoResponse> response = new List<PortFolioInfoResponse>();
            if (db != null)
            {
                try
                {
                    _request.isDeleted = false;
                    _request.createdDate = DateTime.Now;

                    db.PortfolioInfo.Add(_request);
                    var result = await db.SaveChangesAsync();

                    var entity = db.PortfolioInfo.Where(item => item.profileID == _request.profileID).ToList();
                    if (entity != null)
                    {
                        foreach (var item in entity)
                        {
                            PortFolioInfoResponse res = new PortFolioInfoResponse();
                            res.portFolioID = item.portFolioID;
                            res.profileID = item.profileID;
                            res.userID = item.userID;
                            res.portFolioPath = item.portFolioPath;

                            response.Add(res);

                        }
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return response;
        }
        public async Task<List<PortFolioInfoResponse>> GetPortfolioInfoByUser(int resquedID)
        {
            List<PortFolioInfoResponse> response = new List<PortFolioInfoResponse>();
            if (db != null)
            {
                try
                {


                    var entity = db.PortfolioInfo.Where(item => item.userID == resquedID && item.isDeleted == false).ToList();
                    if (entity != null)
                    {
                        foreach (var item in entity)
                        {
                            PortFolioInfoResponse res = new PortFolioInfoResponse();
                            res.portFolioID = item.portFolioID;
                            res.profileID = item.profileID;
                            res.userID = item.userID;
                            res.portFolioPath = item.portFolioPath;

                            response.Add(res);

                        }
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return response;
        }

        public async Task<List<PortFolioInfoResponse>> GetPortfolioInfo(int resquedID)
        {
            List<PortFolioInfoResponse> response = new List<PortFolioInfoResponse>();
            if (db != null)
            {
                try
                {


                    var entity = db.PortfolioInfo.Where(item => item.profileID == resquedID && item.isDeleted==false).ToList();
                    if (entity != null)
                    {
                        foreach (var item in entity)
                        {
                            PortFolioInfoResponse res = new PortFolioInfoResponse();
                            res.portFolioID = item.portFolioID;
                            res.profileID = item.profileID;
                            res.userID = item.userID;
                            res.portFolioPath = item.portFolioPath;

                            response.Add(res);

                        }
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return response;
        }
        public async Task<bool> RemovePortfolioInfo(int portFolioID, int profileID)
        {
            bool status = false;
            if (db != null)
            {
                try
                {
                    var entity = db.PortfolioInfo.FirstOrDefault(item => item.portFolioID == portFolioID && item.profileID == profileID && item.isDeleted == false);
                    if (entity != null)
                    {
                        entity.isDeleted = true;
                        db.PortfolioInfo.Update(entity);

                        var res = await db.SaveChangesAsync();
                        if (res > 0)
                        {
                            status = true;
                            return status;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return status;
        }
        public async Task<string> SaveCardInfo(cardStripeRqst objRqst, string secretKey)
        {
            try
            {
                var entity = db.cardStripe.Where(x => x.userid == objRqst.userid).FirstOrDefault();
                var userDetail = db.SignUpRequest.Where(x => x.id == objRqst.userid).FirstOrDefault();
                if (entity == null)
                {

                    var customerId = db.customerStripe.Where(x => x.userid == objRqst.userid).FirstOrDefault();
                    StripeConfiguration.ApiKey = secretKey;
                    var options = new CardCreateOptions
                    {
                        Source = "tok_visa",
                    };
                    var service = new CardService();
                    var cardServ = service.Create(customerId.customer_stripe_id, options);
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
                    obj.token_id = objRqst.token_id;
                    obj.customer_stripe_id = customerId.customer_stripe_id;
                    obj.card_id = cardServ.Id;
                    obj.userid = objRqst.userid;
                    obj.type = objRqst.type;
                    obj.account_id = accountDetail.Id;
                    db.cardStripe.Add(obj);
                    db.SaveChanges();
                    return "Card Information Updated";
                }
                else
                {
                    return "Card Information Already Updated";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
