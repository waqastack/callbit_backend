using Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IProfileService
    {
        Task<string> GetProfilePicture(int id, string secretKey);
        Task<ProfilePicture> UploadProfilePicture(ProfilePicture res);
        Task<List<profileInformation>> GetProfileInfo(int id);
        Task<ProfileInfoResponse> ProfileInfo(profileInformation _request);
        Task<List<PortFolioInfoResponse>> PortfolioInfo(PortFolioInfoRequest _request);
        Task<List<PortFolioInfoResponse>> GetPortfolioInfo(int resquedID);
        Task<List<PortFolioInfoResponse>> GetPortfolioInfoByUser(int resquedID);
        Task<bool> ProfileInfoOnSignUp(int _userID, string _userName);
        Task<bool> RemovePortfolioInfo(int portFolioID, int profileID);
        Task<string> SaveCardInfo(cardStripeRqst obj, string secretKey);
    }
}
