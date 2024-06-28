using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IDBService
    {
        Task<SignUpRequest> RegisterUser(SignUpRequest s, string secretKey);
        Task<string> CheckUserNameDuplication(string _userName);
        Task<SignUpRequest> Login(LoginRequest r);
        Task<Compaign> PostCompaign(CompaignRequest s);

        Task<List<Compaign>> GetAllCompaign();
        Task<List<SignUpRequest>> GetAllCallCentreUsers();
        Task<List<SignUpRequest>> GetSingleUser(int id);
        Task<List<SignUpRequest>> GetAllUsers();
        bool IsEmailUsed(string email);
        Task<SignUpRequest> GetUser(string email);
        Task<List<SignUpRequest>> GetAllClients();
        bool deleteData(int id);
        Task<List<SignUpRequest>> UpdateUser(int id, SignUpRequest teacher);
        Task<List<Compaign>> GetSingleCompaign(int id);
        Task<MyCampaignResponse> GetLoginUserCompaign(MyCampaignRqst model);
        Task<string> SubmitProposal(SubmitProposal s, List<CampaignAnswer> rst);
        Task<string> ChangePassword(ChangePasswordRequest s);
        Task<string> ChangeType(SignUpRequest signUpRequest);
        Task<List<UserAllow>> GetUserAllow(UserAllowReq s);
        Task<string> AddUserAllow(UserAllowReq s);
        Task<List<ProposalDetails>> GetDetailsData(int id);
        Task<string> CheckEmail(string email);
        Task<List<ProfileInfoPopup>> getProfileInfo(int id);
        Task<ContractsClient> GetContractLists();
        Task<string> CheckAlreadyProposal(CheckProposalReq model);
        Task<CampaignRecievedAmountViewModel> GetAllRecievedAmount();
        Task<SendAmountCallCenterViewModel> GetPendingSendAmount();
        Task<string> TransferAmount(int id);
        Task<string> deleteCompaignInfo(int compaignID);
    }
}