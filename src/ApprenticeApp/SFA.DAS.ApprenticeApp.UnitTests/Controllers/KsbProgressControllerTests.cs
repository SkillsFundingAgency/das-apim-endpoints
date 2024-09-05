using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class KsbProgressControllerTests
    {

        [Test, MoqAutoData]
        public async Task AddUpdateKsbProgress_Test(
            [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            ApprenticeKsbProgressData data = new()
            {
                ApprenticeshipId = 1,
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

            var result = await controller.AddUpdateKsbProgress(apprenticeshipId, data);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task RemoveTaskToKsbProgress_test(
            [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            int ksbProgressId = 1;
            int taskId = 2;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.RemoveTaskToKsbProgress(apprenticeshipId, ksbProgressId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }

        [Test, MoqAutoData]
        public async Task GetTaskKsbProgress(
           [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            Guid[] guids = { Guid.NewGuid() };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetKsbsByApprenticeshipIdAndGuidListQuery(apprenticeshipId, guids);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsbs_NoKsbs_Test(
            Mock<IMediator> mockMediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
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

        [Test, MoqAutoData]
        public async Task GetKsbProgressForTask(
           [Greedy] KsbProgressController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            var taskId = 1;

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetKsbProgressForTask(apprenticeshipId, taskId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsb_Test(
           Mock<IMediator> mockMediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
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

            var result = await controller.GetApprenticeshipKsbProgress(apprenticeshipId, standardUid, option, testKsb.Id);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsb_NoKsb_Test(
           Mock<IMediator> mockMediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
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

            var testKsbProgress = new ApprenticeKsbProgressData
            {
                ApprenticeshipId = apprenticeshipId,
                CurrentStatus = KSBStatus.InProgress,
                KSBId = testKsb.Id,
                KsbKey = testKsb.Key,
                KSBProgressType = KSBProgressType.Knowledge,
                Note = "TestNote"
            };

            ksbQueryResult.KsbsResult.Ksbs.Add(testKsb);

            var ksbProgressResult = new GetKsbsByApprenticeshipIdQueryResult
            {
                KSBProgresses = new System.Collections.Generic.List<ApprenticeKsbProgressData>() { testKsbProgress }
            };


            mockMediator.Setup(m => m.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(ksbQueryResult);
            mockMediator.Setup(m => m.Send(It.IsAny<GetKsbsByApprenticeshipIdQuery>(), default)).ReturnsAsync(ksbProgressResult);

            var controller = new KsbProgressController(mockMediator.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbProgress(apprenticeshipId, standardUid, option, testKsb.Id);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_ApprenticeKsb_NoKsbResult_Test(
          Mock<IMediator> mockMediator)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeshipId = 1;
            string standardUid = "TestStandardUid";
            string option = "core";
            Guid ksbId = Guid.NewGuid();

            var ksbQueryResult = new GetStandardOptionKsbsQueryResult();
           
            mockMediator.Setup(m => m.Send(It.IsAny<GetStandardOptionKsbsQuery>(), default)).ReturnsAsync(ksbQueryResult);

            var controller = new KsbProgressController(mockMediator.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetApprenticeshipKsbProgress(apprenticeshipId, standardUid, option, ksbId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkResult));
        }
    }
}