using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class ProposalDetails
    {
        [Key]
        public int submitted_proposals { get; set; }
        public int available_proposals { get; set; }
    }
}
