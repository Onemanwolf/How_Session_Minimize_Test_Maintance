using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Maintance_DemoCode
{
    public class EmailGateway : IEmailGateway
    {
        public void Send(EmailMessage message)
        {
            // simulate sending email
            Debug.WriteLine("Sending email to: " + message.ToAddress);
        }
    }
}
