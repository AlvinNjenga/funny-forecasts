using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010;
using Twilio.Rest.Api.V2010.Account;

namespace FunnyForecasts
{
    public static class NotificationService
    {
        private static string TWILIO_ACCOUNT_SID { get;  }
        private static string TWILIO_AUTH_TOKEN { get;  } 

        public static void SendSMS(string message)
        {
            TwilioClient.Init(TWILIO_ACCOUNT_SID, TWILIO_AUTH_TOKEN);

            var notification = MessageResource.Create(
                body: message,
                from: new Twilio.Types.PhoneNumber("+16515041169"),
                to: new Twilio.Types.PhoneNumber("+NUMBER")
            );

            Console.WriteLine(notification.Sid);
        }
    }
}
