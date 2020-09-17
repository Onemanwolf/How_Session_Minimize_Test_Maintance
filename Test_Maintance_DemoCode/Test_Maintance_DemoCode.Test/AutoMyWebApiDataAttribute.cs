using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Maintance_DemoCode.Test
{
    public class AutoMyWebApiDataAttribute : AutoDataAttribute
    {
        public AutoMyWebApiDataAttribute()
            : base(() => new Fixture().Customize(new MyWebApiCustomization()))
        {
        }
    }

    public class MyWebApiCustomization : CompositeCustomization
    {
        public MyWebApiCustomization()
            : base(
                new HttpSchemeCustomization(),
                new ApiControllerCustomizationMeta(),
                new AutoMoqCustomization()

            )
        {
        }
    }
        public class HttpSchemeCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Inject(new UriScheme("http"));
            }
        }

        public class ApiControllerCustomizationMeta : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Inject(new ViewDataDictionary(fixture.Create<ModelMetadataProvider>(), fixture.Create<ModelStateDictionary>()));
            }

        }



}
