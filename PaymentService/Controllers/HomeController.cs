using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using PaymentService.Helper;
using PaymentService.Models;
using PaymentService.Repository;

namespace PaymentService.Controllers
{
  [ApiController]
  public class HomeController : Controller
  {
    private IMakePayment _makePayment;
    private ValidateUser _validateUser;
    private IConfiguration _configuration;
    private string secretKey;

    public HomeController(IMakePayment makePayment, IConfiguration configuration)
    {
      _makePayment = makePayment;
      _validateUser = new ValidateUser();
      _configuration = configuration;
      secretKey = PaymentServiceUtils.SecretKey;
    }

    [Route("api/paymentservice/pay")]
    public async Task<IActionResult> Payment(PaymentModel payment)
    {
      if (_validateUser.ValidatePaymentParameters(payment))
      {
        string status = await _makePayment.PayAsync(payment, secretKey);
        if (status != "Success")
          return BadRequest(status);
        return Ok(status);
      }

      return BadRequest("Wrong parameters!");
    }


    [Route("api/paymentservice/validate")]
    public async Task<IActionResult> ValidateCard(PaymentModel payment)
    {
      if (_validateUser.ValidateCardParameters(payment))
      {
        string r = await _makePayment.ValidateCard(payment, secretKey);
        if (r == "Success")
          return Ok("Card is Valid");
        else
          return BadRequest(r);
      }

      return BadRequest("Wrong parameters!");
    }
  }
}
