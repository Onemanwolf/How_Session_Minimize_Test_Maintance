# Test Maintenance And Best Practices

## AutoFixture XUnit 2

`<PackageReference Include="AutoFixture.Xunit2" Version="4.14.0" />`

Unit Testing best practices:

- Naming conventions
  - Intention revealing should state clearly what the method, class, property is doing or what it is for so follow on maintainers can quickly understand what is going on.
- Class Names

  - ClassName_Should
  - ClassName_Test
  - ClassName.Test

- Unit Test names

  - WhatIsBeingTested_WhatIsExpected_Inputs
  - Example: `Update_DTO_ReturnTrue`

- When you test and what to test

  - Always focus on Business Rules or Business logic
  - Realistic Code coverage should be around 75% some say more but most agree that around the 80% and above you start to realize diminishing returns
  - Critical Dependencies of the application.

- Should you have more than one assert and if so should it be moved over to another test.

  - One Assert pre test is highly recommended but not a hard rule if say all the extra assert are asserting on the same objects properties for example; but, I recommend one per test.

- How to analyze code coverage?
  - [Code Coverage .Net](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=windows)
  - Test Explore can tell you the percentage of [code coverage](https://docs.microsoft.com/en-us/visualstudio/test/using-code-coverage-to-determine-how-much-code-is-being-tested?view=vs-2019)
  - [Mutation testing](https://en.wikipedia.org/wiki/Mutation_testing)
  - [Mutator Stryker](https://stryker-mutator.io/)
- Any tools recommendations and we already have ReSharper which I think has some support for this as well.

  - [ReSharper Code Coverage](https://www.jetbrains.com/dotcover/?var=landing&gclsrc=aw.ds&&gclid=CjwKCAjw74b7BRA_EiwAF8yHFJiHR-vpWU8kLmdwqQ98fx03nfvIcjRVgc4R2rWsPqJ6k49mIC-97RoCfoIQAvD_BwE)
  - Providing completed code for reference for the examples would be great like it was provided initially.

- More explanation about why certain approaches were taken in the examples to better understand how to think about it from our end.
  - Lets take a look at your code
- How the different classes in the examples interact with each other.

## Patterns

- Specification
  - Specification pattern is a pattern that allows us to encapsulate some portion of domain knowledge into a single object - specification - and reuse it in different parts of the code base.
  - It can hold business rules that returns a boolean
  - It can ensure build object in the right state before persisting them
  - It can be used to reconstitute and object from the database into a object in the correct state enforced by domain rule and logic.

Lets take a look at an example and feel free to try it out

Go to the `Test_Maintance_DemoCode` project;

First we create a interface of type T the `IsSatisfied` method with supporting DateTime interface.

```C#
using System;
using System.Text;
using System.Linq;

namespace Test_Maintance_DemoCode
{
    public interface ISpecification<T>
    {
        bool IsSatisfied(T type);
    }

    public interface IDateTime
    {
        DateTime CurrentDate { get; set; }
    }

    public class CurrentDateTime : IDateTime
    {
        DateTime IDateTime.CurrentDate { get; set; } = DateTime.Now;
    }

}

```
Now we implement the `ISpecification<Invoice>` for a business rule for delinquent invoices with a class called `DelinquentInvoiceSpecification`.

```C#
using System;
using System.Collections.Generic;

namespace Test_Maintance_DemoCode
{
    public class DelinquentInvoiceSpecification : ISpecification<Invoice>
    {
        private IDateTime _currentDate;
        private IsGoodStandingSpecification<Customer> _isGoodStanding = new IsGoodStandingSpecification<Customer>();

        public DelinquentInvoiceSpecification(IDateTime currentDate)
        {
            _currentDate = currentDate;
        }

        public bool IsSatisfied(Invoice invoice)
        {
            var gracePeriodDays = 20.0;
            var goodStandingGracePeriodDays = 60;
            TimeSpan diffResult = _currentDate.CurrentDate.ToUniversalTime().Subtract(invoice.InvoiceDate.ToUniversalTime());
            var check = diffResult.TotalDays;


            if (_isGoodStanding.IsSatisfied(invoice.Customer))
            {
                if(check > goodStandingGracePeriodDays)
                {
                    invoice.Customer.GoodStanding = false;
                    return true;
                }
                else
                {

                    return false;
                }
            }
            else
            {
                if (check > gracePeriodDays)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

    }

}


```


Lets add the Customer is in good standing rule spec class called `IsGoodStandingSpecification` which `DelinquentInvoiceSpecification` depends on.


```C#
          public class IsGoodStandingSpecification<T> : ISpecification<Customer>
          {
             public bool IsGoodStanding(Customer customer)
             {

            bool isGoodStanding;

            //Good standing rules

            if (customer.GoodStanding == true && customer.IsMembershipPaid == true)
            {
                isGoodStanding = true;
            }
            else
            {
                isGoodStanding = false;
            }


            if (isGoodStanding)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsSatisfied(Customer type)
        {
            return IsGoodStanding(type);
        }
    }

```

Now add our customer class and Invoice class.

```C#
        namespace Test_Maintance_DemoCode
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set;}

        public bool GoodStanding { get; set;}

        public bool IsMembershipPaid { get; set;}
    }
}

using System;
using System.Collections.Generic;

namespace Test_Maintance_DemoCode
{
    public class Invoice
    {
        public string InvoiceId { get; set;}
        public DateTime InvoiceDate { get; set; }
        public double Amount { get; set; }
        public Customer Customer {get; set;}
        public List<Item> Items { get; set; }



    }

    public class Item
    {
        public int Id { get; set;}
        public string ItemName { get; set; }
        public string Discription { get; set; }

    }
}
```

Lets go to the Test Project and Create a Test Class called `DelinquentInvoiceSpecificationShould`

```C#
        public class DelinquentInvoiceSpecificationShould
            {
        [Fact]
        public void IsDelinquentNotInGoodStanding()
        {
            var mockCurrentDate = new Mock<IDateTime>();
            var fixture = new Fixture();
            mockCurrentDate.Setup(x => x.CurrentDate).Returns(DateTime.Now);
            var customer = fixture.Build<Customer>().With(x => x.GoodStanding, false).Create();
            var sut = new DelinquentInvoiceSpecification(mockCurrentDate.Object);
            var invoice = fixture.Build<Invoice>().With(x => x.InvoiceDate, DateTime.Now.AddDays(-30)).With(x => x.Customer, customer).Create();

            var results = sut.IsSatisfied(invoice);

            results.Should().Be(true);
        }

        [Fact]
        public void NotDelinquentGoodStandingCustomer()
        {
            var mockCurrentDate = new Mock<IDateTime>();
            var fixture = new Fixture();
            var customer = fixture.Build<Customer>().With(x => x.GoodStanding, true).With(x => x.IsMembershipPaid, true).Create();
            mockCurrentDate.Setup(x => x.CurrentDate).Returns(DateTime.Now);
            var sut = new DelinquentInvoiceSpecification(mockCurrentDate.Object);
            var invoice = fixture.Build<Invoice>().With(x => x.InvoiceDate, DateTime.Now.AddDays(-30)).With(x => x.Customer, customer).Create();

            var results = sut.IsSatisfied(invoice);

            results.Should().Be(false);
        }

        [Fact]
        public void NotDelinquentNotInGoodStanding()
        {
            //Arrange
            var mockCurrentDate = new Mock<IDateTime>();
            var fixture = new Fixture();
            mockCurrentDate.Setup(x => x.CurrentDate).Returns(DateTime.Now);
            var customer = fixture.Build<Customer>().With(x => x.CustomerId, 56).With(x => x.GoodStanding, false).With(x => x.IsMembershipPaid, false).Create();
            var sut = new DelinquentInvoiceSpecification(mockCurrentDate.Object);
            var invoice = fixture.Build<Invoice>().With(x => x.InvoiceDate, DateTime.Now.AddDays(-15)).With(x => x.Customer, customer).Create();

            //Act
            var results = sut.IsSatisfied(invoice);
            //Assert
            results.Should().Be(false);
        }


        [Fact]
        public void NotDelinquent_InGoodStanding_ButPastedGoodStandingGracePeriod_UpdateGoodStandingToFalse()
        {
            var mockCurrentDate = new Mock<IDateTime>();
            var fixture = new Fixture();
            mockCurrentDate.Setup(x => x.CurrentDate).Returns(DateTime.Now);
            var customer = fixture.Build<Customer>().With(x => x.GoodStanding, true).Create();
            var sut = new DelinquentInvoiceSpecification(mockCurrentDate.Object);
            var invoice = fixture.Build<Invoice>().With(x => x.InvoiceDate, DateTime.Now.AddDays(-65)).With(x => x.Customer, customer).Create();

            var results = sut.IsSatisfied(invoice);

            invoice.Customer.GoodStanding.Should().Be(false);
        }

    }
```
```

## AutoMoq

AutoFixture.AutoMoq

All the packages should be added to the test project.



## Refactoring

Replace Manual created data with Auto.Fixture

Add the below test methods to the `EmailMessageBufferShould`

```C#

        [Fact]
        public void AddMessageToBuffer()
        {
            var sut = new EmailMessageBuffer();

            var message = new EmailMessage("tioleson@microsoft.com",
                                           "Hi, hope you are good today, Don't forget to tell Wen to feed the birds",
                                           true);


            sut.Add(message);

            Assert.Equal(1, sut.UnsentMessagesCount);
        }


        [Fact]
        public void RemoveMessageFromBufferWhenSent()
        {
            var sut = new EmailMessageBuffer();

            var message = new EmailMessage("tioleson@microsoft.com",
                                           "Hi, hope you are good today, Timmy",
                                           true);

            sut.Add(message);


            sut.SendAll();

            Assert.Equal(0, sut.UnsentMessagesCount);
        }


        [Fact]
        public void SendOnlySpecifiedNumberOfMessages()
        {
            var sut = new EmailMessageBuffer();

            var message1 = new EmailMessage("tioleson@microsoft.com",
                                           "Hi, hope you are good today, remember to add the completed code examples",
                                           true);


            var message2 = new EmailMessage("tioleson@microsoft.com",
                                            "Hi, hope you are good today, Taco Tuesday is my favorite day",
                                            true);

            var message3 = new EmailMessage("tioleson@microsoft.com",
                                            "Hi, Sally is so slow she is flooding Florida",
                                            true);

            sut.Add(message1);
            sut.Add(message2);
            sut.Add(message3);

            sut.SendLimited(2);

            Assert.Equal(1, sut.UnsentMessagesCount);
        }

```

Manual Data creation makes our test brittle we can see this by make a little change to the `EmailMessageBuffer`. But adding a new parameter to the constructor `subject`

```C#
 public EmailMessage(string toAddress, string messageBody, bool isImportant, string subject)
        {
            ToAddress = toAddress;
            MessageBody = messageBody;
            IsImportant = isImportant;
            Subject = subject;
        }

```

You will noticed that the test are now broken and you will have to go add the subject to each broken test.

But this is why we use `Auto.Fixture` in our test because it can easily adapt to this new change.

Lets refactor our test to make then more resilient by implementing Auto.Fixture.

```C#
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

```

If we need to add new parameters of Email message constructor our test will not break now awesome great job.

## parameterized Test

Now let look a passing parameters to test methods using `[InlineAutoData]`

```C#
        using AutoFixture.Xunit2;

```

> Pro Tip:
> [Add New File](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.AddNewFile):
> A Visual Studio extension for easily adding new files to any project. Simply hit Shift+F2 to create an empty file in the selected folder or in the same folder as the selected file.

Now go to the test class called `CalculatorShould`. and add some test methods

```C#
         [Fact]
        public void AddTwoPositiveNumbers()
        {
            var sut = new Calculator();

           var value = sut.Add(1 , 2);


            Assert.Equal(3, value) ;
        }


        [Fact]
        public void AddZeroAndPositiveNumber()
        {
            var sut = new Calculator();

           var value =  sut.Add(0, 2);


            Assert.Equal(2, value);
        }


        [Fact]
        public void AddNegativeAndPositiveNumber()
        {
            var sut = new Calculator();

           var value = sut.Add(-5, 1);


            Assert.Equal(-4, value);
        }

```

As you can see we have hard coded the values in and we have 3 test methods to maintain.

## Data Driven Test

We can use `[InlineAutoData]` and change the test method from Fact to Theory. As you can see below if we need to use negative numbers `[InlineAutoData(-5)]` or zero `[InlineAutoData(0)]` now we only have one test method to maintain.

Additionally we don't need to new up Calculator instance `sut = new Calculator()` as InlineAutoData provides one for us thats awesome.

lets lose the two bottom test methods by deleting them and use one test method to test all the case we have and and just two lines of code.

```C#
        [Theory]
        [InlineAutoData(0)]
        [InlineAutoData]
        [InlineAutoData(-5)]
        public void AddTwoNumbers(int a , int b, Calculator sut)
        {
          var value = sut.Add(a, b);

            Assert.Equal(a + b, value);
        }

```

When you run the test now you will see the the below results

```console
        Test_Maintance_DemoCode.Test.CalculatorShould.Add
   Source: CalculatorShould.cs line 20

Test has multiple result outcomes
   3 Passed

Results

    1)  Test_Maintance_DemoCode.Test.CalculatorShould.Add(a: 98, b: 154, sut: Calculator { })
      Duration: 3 ms

    2)  Test_Maintance_DemoCode.Test.CalculatorShould.Add(a: 0, b: 104, sut: Calculator { })
      Duration: < 1 ms

    3)  Test_Maintance_DemoCode.Test.CalculatorShould.Add(a: -5, b: 152, sut: Calculator { })
      Duration: < 1 ms

```

# AutoFixture.AutoMoq;

```C#
        using AutoFixture.AutoMoq;
```

`EmailMessageSendBuffer` requires a `IEmailGateway` we can use a `Mock<IEmailGateway>()` to create the dependency as shown below

We use Moq to mock so lets give that a try add the below test method and make sure you have all the using added.

add the test below and let take a look at how we can improve this test.

```C#
using AutoFixture;
using AutoFixture.AutoMoq;
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
            // Using Mock to create the dependency
            var mockGateway = new Mock<IEmailGateway>();

            var sut = new EmailMessageSendBuffer(mockGateway.Object);

            sut.Add(fixture.Create<EmailMessage>());


            // act
            sut.SendAll();


            // assert
            mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }
```

You may think well why can't I just use `AutoFixture` instead let give that a try and see what happens.

```C#
        [Fact]
        public void SendEmailToGateway_AutoMoq()
        {
            // arrange
            var fixture = new Fixture();

            var sut = fixture.Create<EmailMessageSendBuffer>();

            sut.Add(fixture.Create<EmailMessage>());

            // act
            sut.SendAll();

            // assert

        }

```

```C#
AutoFixture.ObjectCreationExceptionWithPath: 'AutoFixture was unable to create an instance from Test_Maintance_DemoCode.IEmailGateway because it's an interface. There's no single, most appropriate way to create an object implementing the interface, but you can help AutoFixture figure it out.

If you have a concrete class implementing the interface, you can map the interface to that class:

fixture.Customizations.Add(
    new TypeRelay(
        typeof(Test_Maintance_DemoCode.IEmailGateway),
        typeof(YourConcreteImplementation)));

Alternatively, you can turn AutoFixture into an Auto-Mocking Container using your favourite dynamic mocking library, such as Moq, FakeItEasy, NSubstitute, and others. As an example, to use Moq, you can customize AutoFixture like this:

fixture.Customize(new AutoMoqCustomization());

See http://blog.ploeh.dk/2010/08/19/AutoFixtureasanauto-mockingcontainer for more details.

Request path:
	Test_Maintance_DemoCode.EmailMessageSendBuffer
	  Test_Maintance_DemoCode.IEmailGateway emailGateway
	    Test_Maintance_DemoCode.IEmailGateway
'

```

We can remedy this situation using `AutoFixture.AutoMoq` by adding customization `AutoMoqCustomization()` which is part of the `AutoFixture.AutoMoq` namespace.
Also we can test the gateway as before by implementing the `var mockGateway = fixture.Freeze<Mock<IEmailGateway>>();` method.

```C#

        [Fact]
        public void SendEmailToGateway_AutoMoq()
        {
            // arrange
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());

            // so we can test the mockGateway as before we can use Freeze
            var mockGateway = fixture.Freeze<Mock<IEmailGateway>>();

            var sut = fixture.Create<EmailMessageSendBuffer>();

            sut.Add(fixture.Create<EmailMessage>());

            // act
            sut.SendAll();

            // assert
          mockGateway.Verify(x => x.Send(It.IsAny<EmailMessage>()), Times.Once());
        }
```

## Combining Auto Mocking and Auto Data

We are going to add a new class to the test project called `AutoMoqDataAttribute` which will inherit from AutoDataAttribute Func to the `base` class.

```C#

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System;

namespace TestingControllersSample.Test
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
```

the order matters you need to do the freeze before you create the `EmailMessageBuffer` so ordering your parameters correctly is **vital**.

`[Frozen]` is part of the `namespace AutoFixture.Xunit2` and gives the same functionality of the `fixture.Freeze<T>()` method.

```C#

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
```

## Using AutoFixtue to improve mantainance of code

We can find ways to improve our test by using Autofixture to create objects and types for use if do need to ensure the value is right like 3 Uppercase letters we can do with `fixture.Inject(new AirportCode("LHR"))`.

```C#
        [Fact]
        public void CalculateTotalFlightTime()
        {
            // arrange
            var fixture = new Fixture();
            fixture.Inject(new AirportCode("LHR"));
            var sut = fixture.Create<FlightBooking>();


            // etc.


        }

```

So Far we have:

- Refactored our test code to be less brittle
- Parameterized our test to test case with one test minimizing maintance
  - [AutoData]
  - [InlineAutoData]
- Auto Mocked with AutoFixture and Moq
- Combined Auto Mocking and Auto Data
- AutoMoqDataAttribute

## Home Work

Now lets take a look at how you refactored and created your test in your code base and ask some questions.
