using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using AutoFixture.Kernel;
using static Test_Maintance_DemoCode.Test.ApiControllerCustomization;

namespace Test_Maintance_DemoCode.Test
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() => new Fixture().Customize(new AutoMoqCustomization())

            
        
        )
        {

        }
    }
}
