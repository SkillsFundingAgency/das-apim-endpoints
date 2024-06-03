using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class MyApprenticeDetailsControllerTests
    {
        [Test, MoqAutoData]
        public async Task Get_Apprentice_Details_Test(
            [Greedy] MyApprenticeshipController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            var request = new MyApprenticeshipConfirmedRequest();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.Post(apprenticeId, request);
            result.Should().BeOfType(typeof(OkResult));
        }
    }
}