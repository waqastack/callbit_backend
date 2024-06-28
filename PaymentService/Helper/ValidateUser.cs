using PaymentService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Helper
{
    public class ValidateUser
    {
        public bool ValidatePaymentParameters(PaymentModel model)
        {
            if (!string.IsNullOrEmpty(model.cardNumber)
                && !string.IsNullOrEmpty(model.cvc)
                && !string.IsNullOrEmpty(model.userEmail)
                && !string.IsNullOrEmpty(model.expiry)
                && model.value != null
                && !string.IsNullOrEmpty(model.currency))
            {
                if (model.expiry.Contains('/'))
                    return true;
            }

            return false;
        }
        public bool ValidateCardParameters(PaymentModel model)
        {
            if (!string.IsNullOrEmpty(model.cardNumber)
                && !string.IsNullOrEmpty(model.cvc)
                && !string.IsNullOrEmpty(model.expiry))
            {
                if(model.expiry.Contains('/'))
                    return true;
            }

            return false;
        }
    }
}
