using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Api.Controllers;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllCoursesQuery;

namespace SFA.DAS.Roatp.CourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class StandardsControllerTests
    {
        const int ValidUkprn = 101;

        [TestCase(0, 400)]
        [TestCase(-1, 400)]
        [TestCase(ValidUkprn, 200)]
        public async Task GetStandards_ReturnsAppropriateResponse(int ukprn, int expectedStatusCode)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetAllCoursesQuery>(q => q.Ukprn == ValidUkprn), It.IsAny<CancellationToken>())).ReturnsAsync(new List<GetAllCoursesResult>());


            var subject = new StandardsController(Mock.Of<ILogger<StandardsController>>(), mediatorMock.Object);

            var response = await subject.GetAllStandards(ukprn);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual(expectedStatusCode, statusCodeResult.StatusCode.GetValueOrDefault());
        }
    }
}
