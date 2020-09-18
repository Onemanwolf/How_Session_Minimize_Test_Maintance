using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Moq;
using System;
using Xunit;


namespace Test_Maintance_DemoCode.Test
{
    public class EmailMessageSendBufferShould
    {
        [Fact]
        public void SendEmailToGateway_Manual_Moq()
        {
            // arrange
            var fixture = new Fixture();

            var mockGateway = new Mock<IEmailGateway>();

            var sut = new EmailMessageSendBuffer(mockGateway.Object);

            sut.Add(fixture.Create<EmailMessage>());


            // act
            sut.SendAll();


            // assert
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }

        [Fact]
        public void SendEmailToGateway_AutoMoq()
        {
            // arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            var mockGateway = fixture.Freeze<Mock<IEmailGateway>>();

            var sut = fixture.Create<EmailMessageSendBuffer>();

            sut.Add(fixture.Create<EmailMessage>());

            // act
            sut.SendAll();

            // assert
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }

        [Theory]
        [AutoMoqData]
        public void SendEmailToGateway_AutoMoq_WithCustom(EmailMessage message,
                                               [Frozen] Mock<IEmailGateway> mockGateway,
                                               EmailMessageSendBuffer sut)
        {
            // arrange
            sut.Add(message);

            // act
            sut.SendAll();

            // assert
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }
    }
}
