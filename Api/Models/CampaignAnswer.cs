using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    [Table("CampaignAnswers")]
    public class CampaignAnswer
    {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Answer { get; set; }
        public int ProposalId { get; set; }
        public int Compaignid { get; set; }
    }
}
