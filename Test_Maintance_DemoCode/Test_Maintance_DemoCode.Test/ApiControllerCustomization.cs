using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Maintance_DemoCode.Test
{
    public class ApiControllerCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(() => new CustomCompositeMetadataDetailsProvider());
            fixture.Inject(new ViewDataDictionary(fixture.Create<ModelMetadataProvider>(), fixture.Create<ModelStateDictionary>()));
        }

        public class CustomCompositeMetadataDetailsProvider : ICompositeMetadataDetailsProvider
        {
            public void CreateBindingMetadata(BindingMetadataProviderContext context)
            {
                throw new System.NotImplementedException();
            }

            public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
            {
                throw new System.NotImplementedException();
            }

            public void CreateValidationMetadata(ValidationMetadataProviderContext context)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
