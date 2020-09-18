using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Maintance_DemoCode
{
    public class FlightDetails
    {
        public FlightDetails(AirportCode departureAirportCode, AirportCode arrivalAirportCode)
        {
            DepartureAirportCode = departureAirportCode;
            ArrivalAirportCode = arrivalAirportCode;
        }

        public AirportCode DepartureAirportCode { get; }
        public AirportCode ArrivalAirportCode { get; }
        public TimeSpan FlightDuration { get; set; }
        public string AirlineName { get; set; }
    }
}
