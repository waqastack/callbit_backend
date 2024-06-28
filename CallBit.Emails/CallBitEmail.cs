using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace CallBit.Emails
{
    public class CallBitEmail
    {
        private readonly MailAddress _From;
        public string Subject { get; set; }
        public string Body { get; set; }

        #region METHODS

        public CallBitEmail()
        {
            _From = new MailAddress("info@callbit.com", "callbit");
        }
        public bool SendEmail(List<string> emails)
        {
            return SendEmail(_EmailSubject, _EmailBody, emails);
        }
        public bool SendEmail(string email)
        {
            var client = SmtpSettings.Client;
            MailMessage msg = new MailMessage();
            msg.From = _From;
            msg.IsBodyHtml = true;
            msg.Body = this.Body;
            msg.Subject = this.Subject;
            msg.To.Add(new MailAddress(email));
            msg.From = _From;
            client.Send(msg);
            return true;
        }
        public bool SendEmail(string email, string subject, string body)
        {
            try
            {
                var client = SmtpSettings.Client;
                MailMessage msg = new MailMessage();
                msg.From = _From;
                msg.IsBodyHtml = true;
                msg.BodyEncoding = System.Text.Encoding.UTF8;
                msg.SubjectEncoding = System.Text.Encoding.Default;
                msg.IsBodyHtml = true;
                msg.Body = body;
                msg.Subject = subject;
                msg.To.Add(new MailAddress(email));

                msg.From = _From;
                client.UseDefaultCredentials = false;
                client.EnableSsl = true;
                client.Send(msg);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool SendEmail(string subject, string body, List<string> emails)
        {
            try
            {
                var client = SmtpSettings.Client;
                MailMessage msg = new MailMessage();
                msg.From = _From;
                msg.IsBodyHtml = true;
                msg.Body = body;
                msg.Subject = subject;
                msg.From = _From;

                foreach (var email in emails)
                {
                    msg.Bcc.Add(new MailAddress(email));
                }
                SmtpSettings.Client.Send(msg);
                return true;
            }
            catch (Exception e)
            {
            }
            return false;
        }

        #endregion

        #region EMAIL SUBJECT & BODY
        private const string _EmailSubject = "Sign up on Callbit";
        private const string _EmailBody = "Hey I am using CallBit, Join me at <br> https://callbit.com/signup";
        #endregion
    }
}
