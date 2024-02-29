using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProviderCourse;
using System.Threading;
using System.Threading.Tasks;


namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCoursesControllerTests
{
    [TestFixture]
    public class ProviderCoursesControllerGetProviderCourseTests
    {
        const int ValidLarsCode = 101;
        private const int ValidUkprn = 10000001;
        [TestCase(ValidUkprn, 0, 400)]
        [TestCase(ValidUkprn, -1, 400)]
        [TestCase(0, 1, 400)]
        [TestCase(-1, 1, 400)]
        [TestCase(ValidUkprn, ValidLarsCode, 200)]
        public async Task GetProviderCourse_ReturnsExpectedState(int ukprn, int larsCode, int expectedStatusCode)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderCourseQuery>(q => q.LarsCode == larsCode && q.Ukprn == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderCourseResult { LarsCode = ValidLarsCode });

            var controller = new ProviderCoursesController(Mock.Of<ILogger<ProviderCoursesController>>(), mediatorMock.Object);

            var response = await controller.GetProviderCourse(ukprn, larsCode);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.That(expectedStatusCode, Is.EqualTo(statusCodeResult.StatusCode.GetValueOrDefault()));
        }

        [Test]
        public async Task GetProviderCourse_Null_ResultReturnsNotFound()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetProviderCourseQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((GetProviderCourseResult)null);

            var controller = new ProviderCoursesController(Mock.Of<ILogger<ProviderCoursesController>>(), mediatorMock.Object);

            var response = await controller.GetProviderCourse(ValidUkprn, ValidLarsCode);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.That(404, Is.EqualTo(statusCodeResult.StatusCode.GetValueOrDefault()));
        }
    }
}
