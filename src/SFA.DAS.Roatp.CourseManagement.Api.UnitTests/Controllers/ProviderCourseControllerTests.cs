using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Api.Controllers;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetProviderCourse;


namespace SFA.DAS.Roatp.CourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderCourseControllerTests
    {
        const int ValidLarsCode = 101;
        private const int ValidUkprn = 10000001;
        private const int ValidProviderCourseId = 1;
        [TestCase(ValidUkprn,0, 0, 400)]
        [TestCase(ValidUkprn, -1, 0, 400)]
        [TestCase(0, 1, 0, 400)]
        [TestCase(-1, 1, 0, 400)]
        [TestCase(ValidUkprn, ValidLarsCode, 200)]
        public async Task GetProviderCourse_ReturnsExpectedState(int ukprn,int larsCode, int providerCourseId, int expectedStatusCode)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderCourseQuery>(q => q.LarsCode == larsCode && q.Ukprn==ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderCourseResult { LarsCode = ValidLarsCode});

            var controller = new ProviderCourseController(Mock.Of<ILogger<ProviderCourseController>>(), mediatorMock.Object);

            var response = await controller.GetProviderCourse(ukprn, larsCode, providerCourseId);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual(expectedStatusCode, statusCodeResult.StatusCode.GetValueOrDefault());
        }

        [Test]
        public async Task GetProviderCourse_Null_ResultReturnsNotFound()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetProviderCourseQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync( (GetProviderCourseResult) null);

            var controller = new ProviderCourseController(Mock.Of<ILogger<ProviderCourseController>>(), mediatorMock.Object);

            var response = await controller.GetProviderCourse(ValidUkprn, ValidLarsCode, ValidProviderCourseId);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual(404, statusCodeResult.StatusCode.GetValueOrDefault());
        }
    }
}
