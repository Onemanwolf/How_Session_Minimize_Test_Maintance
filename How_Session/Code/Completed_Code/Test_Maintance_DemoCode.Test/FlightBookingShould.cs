using AutoFixture;
using Xunit;

namespace Test_Maintance_DemoCode.Test
{
    public class FlightBookingShould
    {
        [Fact]
        public void CalculateTotalFlightTime()
        {
            // arrange
            var fixture = new Fixture();
            fixture.Inject(new AirportCode("LHR"));
            var sut = fixture.Create<FlightBooking>();


            // etc.

            
        }
    }
}
