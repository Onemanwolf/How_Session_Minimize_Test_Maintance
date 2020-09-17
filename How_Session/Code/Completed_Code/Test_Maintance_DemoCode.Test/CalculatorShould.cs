using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using AutoFixture;
using FluentAssertions;
using AutoFixture.Xunit2;
using System.ComponentModel.DataAnnotations;

namespace Test_Maintance_DemoCode.Test
{
    public class CalculatorShould
    {
        [Theory]
        [InlineAutoData] // AddTwoPositiveNumbers
        [InlineAutoData(0)] // AddZeroAndPositiveNumber
        [InlineAutoData(-5)] // AddNegativeAndPositiveNumber
        public void Add(int a, int b, Calculator sut)
        {
           var value =  sut.Add(a, b);
           

            Assert.Equal(a + b, value);
        }
    }
}
