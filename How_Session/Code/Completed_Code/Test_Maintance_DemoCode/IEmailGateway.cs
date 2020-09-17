using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Maintance_DemoCode
{
    public interface IEmailGateway
    {
        void Send(EmailMessage message);
    }
}
