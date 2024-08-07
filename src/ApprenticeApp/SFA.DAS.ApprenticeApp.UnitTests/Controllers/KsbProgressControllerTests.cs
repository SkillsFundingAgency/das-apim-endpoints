using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Framing;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class KsbProgressControllerTests
    {

        [Test, MoqAutoData]
        public async Task AddUpdateKsbProgress_Test(
            [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            ApprenticeKsbProgressData data = new()
            {
                ApprenticeshipId = new Guid(),
                CurrentStatus = 1,
                KSBId = new Guid(),
                KsbKey = "key",
                KSBProgressType = 1,
                Note = "note"
            };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.AddUpdateKsbProgress(apprenticeId, data);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task RemoveTaskToKsbProgress_test(
            [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            int ksbProgressId = 1;
            int taskId = 2;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.RemoveTaskToKsbProgress(apprenticeId, ksbProgressId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

    }
}