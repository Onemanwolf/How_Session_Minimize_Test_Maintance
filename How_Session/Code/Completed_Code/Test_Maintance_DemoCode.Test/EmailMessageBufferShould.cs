using Xunit;
using AutoFixture;

namespace Test_Maintance_DemoCode.Test
{
    public class EmailMessageBufferShould
    {
        [Fact]
        public void AddMessageToBuffer()
        {
            var fixture = new Fixture();
            var sut = new EmailMessageBuffer();

            sut.Add(fixture.Create<EmailMessage>());

            Assert.Equal(1, sut.UnsentMessagesCount);
        }


        [Fact]
        public void RemoveMessageFromBufferWhenSent()
        {
            var fixture = new Fixture();
            var sut = new EmailMessageBuffer();

            sut.Add(fixture.Create<EmailMessage>());

            sut.SendAll();

            Assert.Equal(0, sut.UnsentMessagesCount);
        }


        [Fact]
        public void SendOnlySpecifiedNumberOfMessages()
        {
            var fixture = new Fixture();
            var sut = new EmailMessageBuffer();

            sut.Add(fixture.Create<EmailMessage>());
            sut.Add(fixture.Create<EmailMessage>());
            sut.Add(fixture.Create<EmailMessage>());

            sut.SendLimited(2);

            Assert.Equal(1, sut.UnsentMessagesCount);
        }
    }
}
