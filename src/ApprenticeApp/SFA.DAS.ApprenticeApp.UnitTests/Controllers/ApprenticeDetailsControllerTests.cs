using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeAccounts;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.ApprenticeApp.Api.Controllers.ApprenticeDetailsController;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class ApprenticeControllerDetailsTests
    {
        [Test, MoqAutoData]
        public async Task Get_Apprentice_Details_Test(
            [Greedy] ApprenticeDetailsController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            var queryResult = new GetApprenticeQuery
            {
                ApprenticeId = apprenticeId
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeDetails(apprenticeId);
            result.Should().BeOfType(typeof(NotFoundResult));
        }
     
    }
}