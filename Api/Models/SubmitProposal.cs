using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    [Table("SubmitProposal")]
    public class SubmitProposal
    {
        [Key]
        public long proposalID { set; get; }
        public long compaignID { set; get; }
        public long userID { set; get; }
        public long salesRate { set; get; }
        public long numberofSales { set; get; }
        public long clientRecive { set; get; }
        public string coverLetter { set; get; }
        public string uploadFile { set; get; }
        public bool? isDeleted { set; get; }
        public DateTime createdDate { set; get; }
        public string proposalStatus { get; set; }
        public int? clientID { get; set; }
    }
}
