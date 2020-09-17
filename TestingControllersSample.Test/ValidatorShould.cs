using AutoFixture;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingControllersSample.ClientModels;
using TestingControllersSample.Infrastructure;
using Xunit;

namespace TestingControllersSample.Test
{
    public class ValidatorShould
    {
        [Fact]
        public void IsValidModel()
        {
            var sut = new Validator<NewIdeaModel>();
            var fixture = new Fixture();
            var order = fixture.Create<NewIdeaModel>();
            var result = sut.IsValid(order);

            result.Should().Be(true);

        }

        [Fact]
        public void IsValidModelFalse()
        {
            var sut = new Validator<NewIdeaModel>();
            var fixture = new Fixture();
            var order = fixture.Build<NewIdeaModel>()
                .Without(x => x.Name)
                .Without(x => x.Description)
                .Create();
            var result = sut.IsValid(order);

            result.Should().Be(false);

        }
    }
}
