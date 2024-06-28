using System.IO;

namespace Api.Helper
{
    public static class EmailAssets
    {
        private static string url = "https://callbit.com/";
        public static string ResetPasswordSubject = "Reset password link";
        public static string VerificationSubject = "Verify your email address";
        public static string InvitationSubject = "Join Callbit";
        public static string WelcomeSubject = "Welcome To CallBit";
        public static string GetResetPasswordBody(string link)
        {
            var folderName = Path.Combine("Resources", "Email");
            var filePath = Path.Combine(folderName, "welcome.html");
            string html = File.ReadAllText(filePath);
            html = html.Replace("**username**", $"please click on link below <br>{link}");
            return html;
        }
        public static string GetVerificationBody(string link)
        {
            var folderName = Path.Combine("Resources", "Email");
            var filePath = Path.Combine(folderName, "index.html");
            string html = File.ReadAllText(filePath);
            html = html.Replace("**linkverify**", link);
            html = html.Replace("**account**", $"<a href='{link}'>Confirm Account</a>");
            //return $"please click on link below to verify email <br> {link}";
            return html;
        }
        public static string GetInvitationBody()
        {
            var folderName = Path.Combine("Resources", "Email");
            var filePath = Path.Combine(folderName, "welcome.html");
            string html = File.ReadAllText(filePath);
            html = html.Replace("**username**", $"Hey I am using CallBit, Join me at <br> {url}signup");
            return html;
        }
        public static string GetWelcomeBody()
        {
            var folderName = Path.Combine("Resources", "Email");
            var filePath = Path.Combine(folderName, "welcome.html");
            string html = File.ReadAllText(filePath);
            html = html.Replace("**username**", $"Hey, Welcome To CallBit, Visit at <br> {url}");
            return html;
        }
    }
}
