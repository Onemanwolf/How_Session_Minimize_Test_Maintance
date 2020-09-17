using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoFixture.AutoMoq;
using AutoFixture;
using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using static Test_Maintance_DemoCode.Test.ApiControllerCustomization;

namespace Test_Maintance_DemoCode.Test
{
    public class AutoApiMoqDataAttribute : AutoDataAttribute
    {
        public AutoApiMoqDataAttribute()
        : base(() =>
        {
            var fixture = new Fixture();

            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());

            return fixture;
        })
        {
        }


        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new CustomCompositeMetadataDetailsProvider());
            fixture.Inject(new ViewDataDictionary(fixture.Create<ModelMetadataProvider>(), fixture.Create<ModelStateDictionary>()));
        }
    }
}
