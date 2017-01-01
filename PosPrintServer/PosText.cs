using System;
using System.Collections.Generic;
using System.Linq;
using Twilio;
using System.Text;
using BOPr;


namespace PosPrintServer
{
    public class PosText
    {
        //public void Main(string[] args)
        //{
        //    //string AccountSid = "AC8bb9f2eb932330488608517b8a4cc5c9";
        //    //string AuthToken = "9ae64949c45be3e0614e3666b7225ed9";
        //    //var twilio = new TwilioRestClient(AccountSid, AuthToken);

        //    //var message = twilio.SendMessage(
        //    //    "+16099210500", "+16096516108",
        //    //    "poo",
        //    //    new string[] { "http://farm2.static.flickr.com/1075/1404618563_3ed9a44a3a.jpg" }

        //    //);
        //    //Console.WriteLine(message.Sid);
        //}

        private string AccountSid = "AC8bb9f2eb932330488608517b8a4cc5c9";
        private string AuthToken = "9ae64949c45be3e0614e3666b7225ed9";
        public PosText()
        {
            //var twilio = new TwilioRestClient(AccountSid, AuthToken);

            //var message = twilio.SendMessage(
            //    "+16099210500", "+16096516108",
            //    "poo",
            //    new string[] { "http://farm2.static.flickr.com/1075/1404618563_3ed9a44a3a.jpg" }

            //);
            //Console.WriteLine(message.Sid);
        }

        public void SendText(string recipientPhone, string textMessage)
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            var message = twilio.SendMessage(
                "+16097853664", recipientPhone,
                textMessage,
                new string[] { "http://farm2.static.flickr.com/1075/1404618563_3ed9a44a3a.jpg" }

            );
        }
        public void SendText(string recipientPhone, PosDoc textMessage)
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            this.SendText(recipientPhone, textMessage.ToString());
            //var message = twilio.SendMessage(
            //    "+16097853664", recipientPhone,
            //    textMessage,
            //    new string[] { "http://farm2.static.flickr.com/1075/1404618563_3ed9a44a3a.jpg" }

            //);
        }

    }
}
