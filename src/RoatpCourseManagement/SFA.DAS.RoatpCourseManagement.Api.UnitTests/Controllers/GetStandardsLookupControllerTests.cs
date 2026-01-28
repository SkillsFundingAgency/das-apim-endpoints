using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardInformation;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetStandardsLookup;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class GetStandardsLookupControllerTests
    {
        [Test]
        public async Task GetAllStandards_ReturnsAppropriateResponse()
        {
            var mediatorMock = new Mock<IMediator>();
            var getAllStandardsResponse = new GetStandardsLookupResponseFromCoursesApi
            { Standards = new List<GetStandardResponseFromCoursesApi> { new() { LarsCode = 235 } } };

            var apiResponse = new ApiResponse<GetStandardsLookupResponseFromCoursesApi>(getAllStandardsResponse, HttpStatusCode.OK, "");

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStandardsLookupQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var subject = new StandardsLookupGetController(Mock.Of<ILogger<StandardsLookupGetController>>(), mediatorMock.Object);

            var response = await subject.GetAllStandards();

            var statusCodeResult = response as IStatusCodeActionResult;

            var okResult = response as OkObjectResult;
            var actualResponse = (GetStandardsLookupResponse)okResult.Value;
            actualResponse!.Standards.Should().BeEquivalentTo(getAllStandardsResponse.Standards, options => options.Excluding(x => x.LarsCode));
            Assert.That(getAllStandardsResponse.Standards[0].LarsCode.ToString(), Is.SameAs(actualResponse.Standards[0].LarsCode));
            Assert.That((int)HttpStatusCode.OK, Is.EqualTo(statusCodeResult!.StatusCode.GetValueOrDefault()));
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        [TestCase(HttpStatusCode.Gone)]
        [TestCase(HttpStatusCode.PermanentRedirect)]
        [TestCase(HttpStatusCode.BadGateway)]
        public async Task GetAllStandards_NonOkResponse_ReturnsErrorResponse(HttpStatusCode statusCode)
        {
            var errorMessage = "Error in retrieval";
            var mediatorMock = new Mock<IMediator>();
            var getAllStandardsResponse = new GetStandardsLookupResponseFromCoursesApi { Standards = null };

            var apiResponse =
                new ApiResponse<GetStandardsLookupResponseFromCoursesApi>(getAllStandardsResponse, statusCode,
                    errorMessage);

            mediatorMock.Setup(m => m.Send(It.IsAny<GetStandardsLookupQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var subject = new StandardsLookupGetController(Mock.Of<ILogger<StandardsLookupGetController>>(), mediatorMock.Object);

            var response = await subject.GetAllStandards();

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.That((int)statusCode, Is.EqualTo(statusCodeResult.StatusCode.GetValueOrDefault()));
            Assert.That(errorMessage, Is.EqualTo(((ObjectResult)statusCodeResult).Value));
        }

        [Test, MoqAutoData]
        public async Task GetStandardInformation_InvokesHandler(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] StandardsLookupGetController sut,
            string larsCode)
        {
            await sut.GetStandardInformation(larsCode);

            mediatorMock.Verify(m => m.Send(It.Is<GetStandardInformationQuery>(q => q.LarsCode == larsCode), It.IsAny<CancellationToken>()));
        }
    }
}
