using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Repository
{
    public interface IMakePayment
    {
        public Task<dynamic> PayAsync(PaymentModel payment, string secretKey);
        public Task<string> ValidateCard(PaymentModel payment, string secretKey);

    }
}
