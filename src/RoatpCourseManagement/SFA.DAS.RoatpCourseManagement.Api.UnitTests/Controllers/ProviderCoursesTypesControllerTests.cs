using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.ProviderCourseTypes.Queries;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProviderCoursesTypesControllerTests
    {
        const int ValidUkprn = 101;
        [TestCase(0, 404)]
        [TestCase(ValidUkprn, 200)]
        public async Task GetProviderCourseTypes_ReturnsAppropriateResponse(int ukprn, int expectedStatusCode)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderCourseTypesQuery>(q => q.Ukprn == ValidUkprn), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProviderCourseTypeResult>());
            var subject = new ProviderCourseTypesController(mediatorMock.Object, Mock.Of<ILogger<ProviderCourseTypesController>>());

            var response = await subject.GetProviderCourseTypes(ukprn);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.That(expectedStatusCode, Is.EqualTo(statusCodeResult!.StatusCode.GetValueOrDefault()));
        }
    }
}
