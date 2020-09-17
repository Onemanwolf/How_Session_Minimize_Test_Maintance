using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;
using FluentAssertions;
using AutoFixture.Xunit2;

namespace Test_Maintance_DemoCode.Test
{
    public class CalculatorShould
    {
        [Theory]
        [InlineAutoData(0)]
        [InlineAutoData]
        [InlineAutoData(-5)]
        public void AddTwoNumbers(int a , int b, Calculator sut)
        {
            sut.Add(a, b);
        }
    }
}
