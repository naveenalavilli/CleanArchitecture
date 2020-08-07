using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
    }

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; set; }


        //public Task SendEmailToAdminAsync(string email, string subject, string message,bool toAdmin)
        //{

        //        return Execute(Options.SendGridKey, "Customer Message", message, "alavillinaveenkumar@gmail.com");
        //}

        // public Task SendEmailAsync(string email, string subject, string message)
        public Task SendEmailAsync(string email, string EmailType, string CallBackURL)
        {
            string subject = string.Empty;
            string message = string.Empty;
            string content = string.Empty;
            switch (EmailType)
            {
                case "CONFIRM":
                    subject = "Confirm Your Email";

                    var url = "URL TO Email Template Text File";

                    //content = (new WebClient()).DownloadString(url);

                    message = "To verify : $EmailValue$ , Click $VerificationEmailValue$";
                    message = message.Replace("$EmailValue$", email).Replace("$VerificationEmailValue$", CallBackURL);
                    break;
                case "RESET":
                    subject = "Reset Your Password";
                    var Reseturl = "URL TO Email Template Text File";

                    //content = (new WebClient()).DownloadString(Reseturl);
                    message = "Click here : $ResetLinkValue$ To Reset";
                    message = message.Replace("$ResetLinkValue$", CallBackURL);
                    break;
                default:
                    subject = "";
                    break;
            }
            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task SendEmailConfirmationAsync(string email, string subject, string EmailConfirmationLink, string EmailToConfirm)
        {

            string message = string.Empty;

            //REad from placeHolder File
            message = message.Replace("$EmailValue$", EmailToConfirm).Replace("$VerificationEmailValue$", EmailConfirmationLink);

            return Execute(Options.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            // return Task.FromResult(SendEmail(subject, message, email));

            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("YourEmail@yopmail.com", "Your Domain"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
