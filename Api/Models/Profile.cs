using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    [Table("profilePicture")]
    public class ProfilePicture
    {
        [Key]
        public long profileId { get; set; }
        public long userID { get; set; }
        public string picturePath { get; set; }
        public bool? isDeleted { set; get; }
        public DateTime createdDate { set; get; }
    }
    [Table("profileInformation")]
    public class profileInformation
    {
        [Key]
        public long profileID { get; set; }
        public long userID { get; set; }
        public string pFullName { get; set; }
        public string pTitle { get; set; }
        public string pDescription { get; set; }
        public string pSittingCapacity { get; set; }
        public string pCompaign { get; set; }
        public bool? isDeleted { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? modifiedDate { get; set; }
    }
    public class ProfileInfoResponse
    {
        public long profileID { set; get; }
        public long userID { set; get; }
        public string pFullName { set; get; }
        public string pTitle { set; get; }
        public string pDescription { set; get; }
        public string pSittingCapacity { set; get; }
        public string pCompaign { set; get; }
    }
    [Table("portFolioInformation")]
    public class PortFolioInfoRequest
    {
        [Key]
        public long portFolioID { set; get; }
        public long userID { set; get; }
        public long profileID { set; get; }
        //public IFormFile[] File { get; set; }  
        public string portFolioPath { set; get; }
        public bool? isDeleted { set; get; }
        public DateTime createdDate { set; get; }
    }
    public class PortFolioInfoResponse
    {
        public long portFolioID { set; get; }
        public long userID { set; get; }
        public long profileID { set; get; }
        public string portFolioPath { set; get; }
    }
    public class PortfolioModel
    {
        public IFormFile[] Files { get; set; }
        public string userID { get; set; }
        public string profileID { get; set; }
        // other properties
    }
    public class ProposalModel
    {
        public string uploadFile { get; set; }
        public long compaignID { set; get; }
        public long userID { set; get; }
        public long salesRate { set; get; }
        public long numberofSales { set; get; }
        public long clientRecive { set; get; }
        public string coverLetter { set; get; }
        public List<CampaignAnswer> Answers { get; set; }
        public string proposalStatus { set; get; }
        public string clientID { set; get; }
        // other properties
    }
    public class ProfilePicRqst
    {
        public int usrID { get; set; }
        public string fileName { get; set; }
    }
    [Table("cardStripe")]
    public class cardStripe
    {
        [Key]
        public int id { get; set; }
        public string customer_stripe_id { get; set; }
        public string card_id { get; set; }
        public string token_id { set; get; }
        public int userid { set; get; }
        public int type { set; get; }
        public string account_id { set; get; }
    }
    public class cardStripeRqst
    {
        public string customer_stripe_id { get; set; }
        public string card_id { get; set; }
        public string token_id { set; get; }
        public int userid { set; get; }
        public int type { set; get; }
    }
}

