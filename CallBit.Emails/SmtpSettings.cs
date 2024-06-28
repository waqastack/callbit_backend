using System.Net;
using System.Net.Mail;

namespace CallBit.Emails
{
  internal class SmtpSettings
  {
    private static int _Port = 587;
    private static int _Timeout = 60000;
    private static string _Host = "smtp.gmail.com";
    private static string _EmailAddress = "info@callbit.com";
    private static string _Username= "info@callbit.com";

    private static string _Password = "Callbit2021!";
    private static SmtpDeliveryMethod _DeliveryMethod = SmtpDeliveryMethod.Network;
    private static bool _DefaultCredentials = false;
    private static bool _EnableSsl = true;

    public static SmtpClient Client
    {
      get => new SmtpClient
      {
        DeliveryMethod = _DeliveryMethod,
        Host = _Host,
        Port = _Port,
        EnableSsl = _EnableSsl,
        Timeout = _Timeout,
        Credentials = new NetworkCredential { /*Domain="whatsales.io",*/UserName = _Username  , Password = _Password }
      };
    }
  }
}
