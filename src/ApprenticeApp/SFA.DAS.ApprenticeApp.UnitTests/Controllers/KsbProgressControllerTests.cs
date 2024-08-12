using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using Contentful.Core.Models;
using Moq;
using MediatR;
using StackExchange.Redis;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;

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
                CurrentStatus = KSBStatus.InProgress,
                KSBId = new Guid(),
                KsbKey = "key",
                KSBProgressType = KSBProgressType.Behaviour,
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

        [Test, MoqAutoData]
        public async Task GetTaskKsbProgress(
           [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            Guid[] guids = { Guid.NewGuid() };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetKsbsByApprenticeshipIdAndGuidListQuery(apprenticeId, guids);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsbs_NoKsbs_Test(
            Mock<IMediator> mockMediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = Guid.NewGuid();
            string standardUid = "TestStandardUid";
            string option = "core";

            var ksbQueryResult = (new GetStandardOptionKsbsQueryResult
            {
                KsbsResult = new GetStandardOptionKsbsResult { Ksbs = new System.Collections.Generic.List<Ksb>() }
            });

            var testKsb = new Ksb
            {
                Id = Guid.NewGuid(),
                Type = KsbType.Knowledge,
                Key = "K1",
                Detail = "TestKnowledgeKsb"
            };

            ksbQueryResult.KsbsResult.Ksbs.Add(testKsb);

            var ksbProgressResult = new GetKsbsByApprenticeshipIdQueryResult
            {
                KSBProgresses = new System.Collections.Generic.List<ApprenticeKsbProgressData>()
            };


            mockMediator.Setup(m => m.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(ksbQueryResult);
            mockMediator.Setup(m => m.Send(It.IsAny<GetKsbsByApprenticeshipIdQuery>(), default)).ReturnsAsync(ksbProgressResult);

            var controller = new KsbProgressController(mockMediator.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbs(apprenticeshipId, standardUid, option);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }
    }
}