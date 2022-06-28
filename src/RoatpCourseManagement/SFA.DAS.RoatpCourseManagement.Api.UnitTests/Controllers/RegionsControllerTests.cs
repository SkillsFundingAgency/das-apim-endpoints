using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Api.Controllers;
using SFA.DAS.Roatp.CourseManagement.Application.Regions.Queries;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class RegionsControllerTests
    {
        [Test]
        public async Task GetStandards_ReturnsAppropriateResponse()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllRegionsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAllRegionsQueryResult());

            var subject = new RegionsController(Mock.Of<ILogger<RegionsController>>(), mediatorMock.Object);

            var response = await subject.GetAllRegions();

            var statusCodeResult = response as IStatusCodeActionResult;
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual((int)HttpStatusCode.OK, statusCodeResult.StatusCode.GetValueOrDefault());
        }
    }
}
