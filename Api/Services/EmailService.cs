using Api.Helper;

using CallBit.Emails;

namespace Api.Services
{
  public class EmailService
  {
    CallBitEmail _CallBitEmail;
    public EmailService(CallBitEmail email)
    {
      _CallBitEmail = email;
    }
    public bool SendResetPasswordEmail(string email, string link)
    {
      return _CallBitEmail.SendEmail(email, EmailAssets.ResetPasswordSubject, EmailAssets.GetResetPasswordBody(link));
    }
    public bool SendVerificationEmail(string email, string link)
    {
      return _CallBitEmail.SendEmail(email, EmailAssets.VerificationSubject, EmailAssets.GetVerificationBody(link));
    }
    public bool SendWelcomEmail(string email)
    {
      return _CallBitEmail.SendEmail(email, EmailAssets.WelcomeSubject, EmailAssets.GetWelcomeBody());
    }
  }
}
