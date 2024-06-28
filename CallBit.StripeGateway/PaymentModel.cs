using System;
namespace CallBit.StripeGateway
{
  public class PaymentModel
  {
    public string cardNumber { get; set; }
    public int? expiryMonth { get { return Convert.ToInt32(expiryDate.Split('/')[0]); }  }
    public int? expiryYear { get { return Convert.ToInt32(expiryDate.Split('/')[1]); } }
    public string expiryDate { get; set; }
    public string cvc { get; set; }
    public int? value { get; set; }
    public string userEmail { get; set; }
  }
}
