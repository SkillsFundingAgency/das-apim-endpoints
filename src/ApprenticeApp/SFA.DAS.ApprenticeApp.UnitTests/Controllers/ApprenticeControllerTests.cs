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

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class ApprenticeControllerTests
    {
        [Test, MoqAutoData]
        public async Task Get_Apprentice_Test(
            [Greedy] ApprenticeController controller)
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

            var result = await controller.GetApprentice(apprenticeId);
            result.Should().BeOfType(typeof(NotFoundResult));
        }
    }
}