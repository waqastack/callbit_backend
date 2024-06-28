using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Models
{
    public class PaymentModel
    {
        public string cardNumber { get; set; }
        public string expiry { get; set; }
        public string cvc { get; set; }
        public int? value { get; set; }
        public string userEmail { get; set; }
        public string currency { get; set; }
    }
}
