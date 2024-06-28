using Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Helper
{
    public class ValidateUser
    {
        public bool ValidarteParameters(PaymentModel model)
        {
            if(!string.IsNullOrEmpty(model.cardNumber) && !string.IsNullOrEmpty(model.cvc)
                && !string.IsNullOrEmpty(model.userEmail) && model.expiryMonth != null
                && model.expiryYear != null && model.value != null)
                return true;

            return false;
        }
    }
}
