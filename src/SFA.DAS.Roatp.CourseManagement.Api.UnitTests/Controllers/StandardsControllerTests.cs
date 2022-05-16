using System.Collections.Generic;
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
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllCoursesQuery;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllStandards;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

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
            mediatorMock
                .Setup(m => m.Send(It.Is<GetAllCoursesQuery>(q => q.Ukprn == ValidUkprn),
                    It.IsAny<CancellationToken>())).ReturnsAsync(new List<GetAllCoursesResult>());


            var subject = new StandardsController(Mock.Of<ILogger<StandardsController>>(), mediatorMock.Object);

            var response = await subject.GetAllStandards(ukprn);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual(expectedStatusCode, statusCodeResult.StatusCode.GetValueOrDefault());
        }


        [Test]
        public async Task GetAllStandards_ReturnsAppropriateResponse()
        {
            var mediatorMock = new Mock<IMediator>();
            var getAllStandardsResponse = new GetAllStandardsResponse
                { Standards = new List<GetStandardResponse> { new GetStandardResponse { LarsCode = 235 } } };

            var apiResponse = new ApiResponse<GetAllStandardsResponse>(getAllStandardsResponse, HttpStatusCode.OK, "");

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllStandardsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var subject = new StandardsController(Mock.Of<ILogger<StandardsController>>(), mediatorMock.Object);

            var response = await subject.GetAllStandards();

            var statusCodeResult = response as IStatusCodeActionResult;

            var okResult = response as OkObjectResult;
            var actualResponse = okResult.Value;
            Assert.AreSame(getAllStandardsResponse, actualResponse);
            Assert.AreEqual((int)HttpStatusCode.OK, statusCodeResult.StatusCode.GetValueOrDefault());
        }

        [Test]
        public async Task GetAllStandards_Exception_ReturnsAppropriateResponse()
        {
            var errorMessage = "Error in retrieval";
            var mediatorMock = new Mock<IMediator>();
            var getAllStandardsResponse = new GetAllStandardsResponse { Standards = null };

            var apiResponse =
                new ApiResponse<GetAllStandardsResponse>(getAllStandardsResponse, HttpStatusCode.BadRequest,
                    errorMessage);

            mediatorMock.Setup(m => m.Send(It.IsAny<GetAllStandardsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var subject = new StandardsController(Mock.Of<ILogger<StandardsController>>(), mediatorMock.Object);

            var response = await subject.GetAllStandards();

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, statusCodeResult.StatusCode.GetValueOrDefault());
            Assert.AreEqual(errorMessage, ((ObjectResult)statusCodeResult).Value);
        }
    }
}
