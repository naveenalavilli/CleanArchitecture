using ApplicationCore.Data.Entities;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Observers
{
    public class EmailObserver : IEmailObserver,IStateObserver
    {
        private readonly IEmailSender _emailSender;

        public EmailObserver(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public void Update(AddressState State)
        {
            //Console.WriteLine("State Id '{0}' is updated to '{1}'. An email sent to customer.", State.StateId, State.Name);
            _emailSender.SendEmailAsync("cleanarch@naveenalavilli.dev", "State Updated", string.Format("State Id '{0}' is updated to '{1}'. An email sent to customer.", State.StateId, State.Name));
        }
    }
}
