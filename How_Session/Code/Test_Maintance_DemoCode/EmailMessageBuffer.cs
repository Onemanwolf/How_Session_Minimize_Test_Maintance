using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Maintance_DemoCode
{
    public class EmailMessageBuffer
    {
        private readonly List<EmailMessage> _emails = new List<EmailMessage>();

        public int UnsentMessagesCount => _emails.Count;

        public void Add(EmailMessage message)
        {
            _emails.Add(message);
        }

        public void SendAll()
        {
            for (int i = 0; i < _emails.Count; i++)
            {
                var email = _emails[i];

                Send(email);
                _emails.Remove(email);
            }
        }

        public void SendLimited(int maximumMessagesToSend)
        {
            var limitedBatchOfMessages = _emails.Take(maximumMessagesToSend).ToArray();

            for (int i = 0; i < limitedBatchOfMessages.Length; i++)
            {
                var email = limitedBatchOfMessages[i];
                Send(email);
                _emails.Remove(email);
            }
        }

        private void Send(EmailMessage email)
        {
            // simulate sending email
            Debug.WriteLine("Sending email to: " + email.ToAddress);
        }
    }
}
