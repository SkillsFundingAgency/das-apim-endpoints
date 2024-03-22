using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllProviderCourses;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProviderCoursesControllerTests
{
    [TestFixture]
    public class ProviderCoursesControllerGetAllProviderCoursesTests
    {
        const int ValidUkprn = 101;
        [TestCase(0, 400)]
        [TestCase(-1, 400)]
        [TestCase(ValidUkprn, 200)]
        public async Task GetAllProviderCourses_ReturnsAppropriateResponse(int ukprn, int expectedStatusCode)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetAllProviderCoursesQuery>(q => q.Ukprn == ValidUkprn), It.IsAny<CancellationToken>())).ReturnsAsync(new List<GetAllProviderCoursesQueryResult>());
            var subject = new ProviderCoursesController(Mock.Of<ILogger<ProviderCoursesController>>(), mediatorMock.Object);

            var response = await subject.GetAllProviderCourses(ukprn);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.That(expectedStatusCode, Is.EqualTo(statusCodeResult.StatusCode.GetValueOrDefault()));
        }

        [Test]
        public async Task GetAllProviderCourses_HandlerReturnsNull_ReturnsNotFound()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProviderCoursesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((List<GetAllProviderCoursesQueryResult>)null);
            var subject = new ProviderCoursesController(Mock.Of<ILogger<ProviderCoursesController>>(), mediatorMock.Object);

            var response = await subject.GetAllProviderCourses(ValidUkprn);

            var statusCodeResult = response as IStatusCodeActionResult;
            Assert.That(StatusCodes.Status404NotFound, Is.EqualTo(statusCodeResult.StatusCode.GetValueOrDefault()));
        }
    }
}
