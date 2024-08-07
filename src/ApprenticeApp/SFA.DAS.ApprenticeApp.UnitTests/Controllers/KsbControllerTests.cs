using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Controllers
{
    public class KsbControllerTests
    {
        [Test, MoqAutoData]
        public async Task Get_TaskBy_Id_Test(
            [Greedy] KsbController controller)
        {
            var httpContext = new DefaultHttpContext();
            string standardUid = "TestStandardUid";
            string option = "core";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetKsbs(standardUid, option);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }
    }
}
