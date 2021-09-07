using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace services
{
    public class AuthMessageSenderOptions
    {
        public string AuthCode { get; set; }
        public string PrivateKey { get; set; }
    }

    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; set; }

        public Task SendEmailAsync(string email, string EmailType, string CallBackURL)
        {
            string subject = string.Empty;
            string message = string.Empty;
            
            switch (EmailType)
            {
                case "CONFIRM":
                    subject = "Confirm Your Email";

                    message = "To verify : $EmailValue$ , Click $VerificationEmailValue$";
                    message = message.Replace("$EmailValue$", email).Replace("$VerificationEmailValue$", CallBackURL);
                    break;
                case "RESET":
                    subject = "Reset Your Password";                  
                    message = "Click here : $ResetLinkValue$ To Reset";
                    message = message.Replace("$ResetLinkValue$", CallBackURL);
                    break;
                default:
                    subject = "";
                    break;
            }
           return SendEmail(Options.AuthCode, Options.PrivateKey, subject, message, email);
        }

        public Task SendEmailConfirmationAsync(string email, string subject, string EmailConfirmationLink, string EmailToConfirm)
        {

            string message = string.Empty;

            //REad from placeHolder File
            message = message.Replace("$EmailValue$", EmailToConfirm).Replace("$VerificationEmailValue$", EmailConfirmationLink);

          return  SendEmail(Options.AuthCode,Options.PrivateKey , subject, message, email);
        }

        /// <summary>
        /// Sending Email using Emailyt email service
        /// which is free!
        /// </summary>
        /// <param name="authCode"></param>
        /// <param name="privateKey"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="email"></param>
        public Task SendEmail(string authCode,string privateKey, string subject, string message, string email)
        {
            
            string baseURL = "https://emailyt.com/";

            string api = "DispatchEmail";

            using var wb = new WebClient();

            var data = new NameValueCollection();

            data["AuthCode"] = authCode;
            data["PrivateKey"] = privateKey;

            data["ToEmail"] = email;
                        
            data["Subject"] = subject;
            data["MailBody"] = message;

            var response = wb.UploadValues(baseURL + api, "POST", data);
            return Task.CompletedTask;
        }
    }
}
