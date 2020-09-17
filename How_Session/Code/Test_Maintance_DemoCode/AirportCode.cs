using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Maintance_DemoCode
{
    public class AirportCode
    {
        private readonly string _code;

        public AirportCode(string code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (!IsValidAirportCode(code))
            {
                throw new ArgumentException("Airport code should be three uppercase letters", nameof(code));
            }

            _code = code;
        }

        public override string ToString()
        {
            return _code;
        }

        private bool IsValidAirportCode(string code)
        {
            var isWrongLength = code.Length != 3;
            var isWrongCase = code != code.ToUpperInvariant();

            if (isWrongLength || isWrongCase)
            {
                return false;
            }

            return true;
        }
    }
}
