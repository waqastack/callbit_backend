using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class Referral
    {
        public int Id { get; set; }
        public SignUpRequest ReferredBy { get; set; }
        public DateTime ReferredOn { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public bool? I { get; set; }
    }
}
