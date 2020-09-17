using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Maintance_DemoCode
{
    public class FlightBooking
    {
        public string BookingReference { get; set; }
        public string CustomerName { get; set; }
        public List<FlightDetails> Legs { get; set; } = new List<FlightDetails>();
    }
}
