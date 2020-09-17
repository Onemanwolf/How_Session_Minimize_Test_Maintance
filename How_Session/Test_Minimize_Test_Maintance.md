# Test Maintenance And Best Practices

## AutoFixture XUnit 2

`<PackageReference Include="AutoFixture.Xunit2" Version="4.14.0" />`

Unit Testing best practices:

- Naming conventions
- Class Names :

  - Unit Test names

- When you test and what to test
- Should you have more than one assert and if so should it be moved over to another test.

  - One Assert pre test is highly recommended but a hard rule

- How to analyze code coverage?
  - Test Explore can tell you the percentage of [code coverage](https://docs.microsoft.com/en-us/visualstudio/test/using-code-coverage-to-determine-how-much-code-is-being-tested?view=vs-2019)
- Any tools recommendations and we already have ReSharper which I think has some support for this as well.
- [ReSharper Code Coverage](https://www.jetbrains.com/dotcover/?var=landing&gclsrc=aw.ds&&gclid=CjwKCAjw74b7BRA_EiwAF8yHFJiHR-vpWU8kLmdwqQ98fx03nfvIcjRVgc4R2rWsPqJ6k49mIC-97RoCfoIQAvD_BwE)
  - Providing completed code for reference for the examples would be great like it was provided initially.
- More explanation about why certain approaches were taken in the examples to better understand how to think about it from our end.
- How the different classes in the examples interact with each other.

## Patterns

- Specification

```C#
using System;
using System.Text;
using System.Linq;

namespace Test_Maintance_DemoCode
{
    public interface ISpecification<T>
    {
        bool IsStatisfied(T type);
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

        public bool IsStatisfied(Invoice invoice)
        {
            var gracePeriodDays = 20.0;
            var goodStandingGracePeriodDays = 60;
            TimeSpan diffResult = _currentDate.CurrentDate.ToUniversalTime().Subtract(invoice.InvoiceDate.ToUniversalTime());
            var check = diffResult.TotalDays;


            if (_isGoodStanding.IsStatisfied(invoice.Customer))
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



## AutoMoq

AutoFixture.AutoMoq

Lets add the packages to the test project.

```xml
        <ItemGroup>
           <PackageReference Include="AutoFixture" Version="4.13.0" />
           <PackageReference Include="AutoFixture.AutoMoq" Version="4.13.0" />
           <PackageReference Include="AutoFixture.Xunit" Version="4.13.0" />
           <PackageReference Include="AutoFixture.Xunit2" Version="4.13.0" />
           <PackageReference Include="FluentAssertions" Version="5.10.3"/
           <PackageReference Include="Microsoft.NET.Test.Sdk" Version="16.5.0" />
           <PackageReference Include="Moq" Version="4.14.5" />
           <PackageReference Include="xunit" Version="2.4.0" />
           <PackageReference Include="xunit.runner.visualstudio" Version="2.4. 0" />
           <PackageReference Include="coverlet.collector" Version="1.2.0" />
         </ItemGroup>
```



## Refactoring

Replace Manual created data with Auto.Fixture

```C#


```



Now go to the test project and add a new test class called `CalculatorShould`.


>Pro Tip:
[Add New File](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.AddNewFile):
A Visual Studio extension for easily adding new files to any project. Simply hit Shift+F2 to create an empty file in the selected folder or in the same folder as the selected file.

Add a new test method called AddTwoNumbers

```C#
        [Theory]
        [InlineAutoData(0)]
        [InlineAutoData]
        [InlineAutoData(-5)]
        public void AddTwoNumbers(int a , int b, Calculator sut)
        {
            sut.Add(a, b);
        }

```
