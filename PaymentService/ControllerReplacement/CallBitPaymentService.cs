using Microsoft.Extensions.Configuration;

using PaymentService.Helper;
using PaymentService.Models;
using PaymentService.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.ControllerReplacement
{
  public class CallBitPaymentService
  {
    private IMakePayment _makePayment;
    private ValidateUser _validateUser;
    private string secretKey;

    public CallBitPaymentService(IMakePayment makePayment)
    {
      _makePayment = makePayment;
      _validateUser = new ValidateUser();
      secretKey = PaymentServiceUtils.SecretKey;
    }

    public bool Payment(PaymentModel payment)
    {
      if (!_validateUser.ValidatePaymentParameters(payment))
        throw new Exception("invalid inputs");
      string status = _makePayment.PayAsync(payment, secretKey).GetAwaiter().GetResult();
      if (status != "Success")
        return false;
      return true;
    }

    public bool ValidateCard(PaymentModel payment)
    {
      if (!_validateUser.ValidateCardParameters(payment))
        throw new Exception("invalid inputs");
      string r = _makePayment.ValidateCard(payment, secretKey).GetAwaiter().GetResult();
      if (r == "Success")
        return true;
      else
        return false;
    }
  }
}
