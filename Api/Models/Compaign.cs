using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    [Table("compaign")]
    public class Compaign
    {
        [Key]
        public int id { set; get; }
        public string title { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public string description { set; get; }
        public string uploadFile { set; get; }
        public string payType { set; get; }
        public string describe { set; get; }
        public string price { set; get; }
        public int? userid { set; get; }
        public DateTime? createdDate { set; get; }
        public bool? isDeleted { set; get; }
        public bool? setPrice { set; get; }
        public long? sale { get; set; }
        public long? totalAmount { get; set; }
        public string payByText { get; set; }
        public string payPerText { get; set; }
        public string compaignText { get; set; }
        public long? amountDeducted { get; set; }
        public string compaignDuration { get; set; }
        public string payPerTextOther { get; set; }
        public string location { get; set; }
        public string FileNames { get; set; }
        public int? noOfProposals { get; set; }
        public bool? contractStart { get; set; }
        public bool? inbound { get; set; }
        public bool? outbound { get; set; }
        public bool? isActive { get; set; }
        public List<CampaignQuestion> Questions { get; set; }
        public Compaign()
        {
            this.Questions = new List<CampaignQuestion>();
        }
    }
    public class CompaignRequest
    {
        public string location { get; set; }
        public string title { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public string description { set; get; }
        public string uploadFile { set; get; }
        public string payType { set; get; }
        public string describe { set; get; }
        public string price { set; get; }
        public int? userID { set; get; }
        public DateTime? createdDate { set; get; }
        public bool? isDeleted { set; get; }
        public bool? setOwnPriceRbtn { set; get; }
        public long? paySale { get; set; }
        public string payPerTextOther { get; set; }
        public long? payPrice { get; set; }
        public long? totalAmount { get; set; }
        public string payByText { get; set; }
        public string payPerText { get; set; }
        public string compaignText { get; set; }
        public long? amountDeducted { get; set; }
        public string compaignDuration { get; set; }
        public string FileNames { get; set; }
        public bool? inbound { get; set; }
        public bool? outbound { get; set; }
        public List<string> Questions { get; set; }
    }
    public class CompaignResponse
    {
        public string title { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public string description { set; get; }
        public string uploadFile { set; get; }
        public string FileNames { get; set; }
        public string payType { set; get; }
        public string describe { set; get; }
        public string price { set; get; }
        public int? userID { set; get; }
        public DateTime? createdDate { set; get; }
        public bool? isDeleted { set; get; }
    }
    public class CompaignEditRequest
    {
        public int id { get; set; }
        public string title { set; get; }
        public string name { set; get; }
        public string type { set; get; }
        public string description { set; get; }
        public string uploadFile { set; get; }
        public string payType { set; get; }
        public string describe { set; get; }
        public string price { set; get; }
        public int paySale { set; get; }
        public int payPrice { set; get; }
        public int sale { get; set; }
        public int totalAmount { set; get; }
        public string payByText { set; get; }
        public string payPerText { set; get; }
        public string compaignText { set; get; }
        public int amountDeducted { set; get; }
        public string compaignDuration { set; get; }
    }
    public class ProfileInfoPopup
    {
        public int userID { get; set; }
        public int profileID { get; set; }
        public string userName { get; set; }
        public string designation { get; set; }
        public string favUserTechn { get; set; }
        public string sittingCapacity { get; set; }
        public string description { get; set; }
    }
    public class CheckProposalReq
    {
        public int UserID { get; set; }
        public int CompaignID { get; set; }
    }
    public class MyCampaignRqst
    {
        public int userID { get; set; }
        public string status { get; set; }
    }
    public class MyCampaignResponse
    {
        public int activeCampaignCount { get; set; }
        public int inactiveCampaignCount { get; set; }
        public List<Compaign> campaign { get; set; }
    }
}
