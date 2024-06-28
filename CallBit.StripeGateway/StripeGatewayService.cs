using Stripe;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallBit.StripeGateway
{
  public class StripeGatewayService
  {
    public async Task<bool> Pay(PaymentModel payment)
    {
        StripeConfiguration.ApiKey = GatewaySettings.SecretKey;

        var tokenOptions = new TokenCreateOptions
        {
          Card = new TokenCardOptions
          {
            Number = payment.cardNumber,
            ExpMonth = payment.expiryMonth,
            ExpYear = payment.expiryYear,
            Cvc = payment.cvc
          }
        };

        var serviceToken = new TokenService();
        Token stripeToken =await serviceToken.CreateAsync(tokenOptions);

        var chargeOptions = new ChargeCreateOptions
        {
          Amount = payment.value,
          Description = "Microservice Test Payment",
          ReceiptEmail = payment.userEmail,
          Currency = "eur",
          Source = stripeToken.Id // the card
        };

        var chargeService = new ChargeService();
        Charge charge = await chargeService.CreateAsync(chargeOptions);

      return charge.Paid;
      }
    }
  
}
