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
using SFA.DAS.Roatp.CourseManagement.Application.RegisteredProviders.Queries;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models.RegisteredProvider;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Roatp.CourseManagement.Api.UnitTests.Controllers
{
    [TestFixture]
    public class RegisteredProvidersControllerTests
    {

        [Test]
        public async Task GetAllRegisteredProviders_ReturnsAppropriateResponse()
        {
            var mediatorMock = new Mock<IMediator>();
            var registeredProviders = new List<RegisteredProviderModel>();

            var apiResponse = new ApiResponse<List<RegisteredProviderModel>>(registeredProviders, HttpStatusCode.OK, "");

            mediatorMock.Setup(m => m.Send(It.IsAny<GetRegisteredProvidersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var subject = new RegisteredProvidersController(Mock.Of<ILogger<RegisteredProvidersController>>(), mediatorMock.Object);

            var response = await subject.GetRegisteredProviders();

            var statusCodeResult = response as IStatusCodeActionResult;

            var okResult = response as OkObjectResult;
            var actualResponse = okResult.Value;
            Assert.AreSame(apiResponse.Body, actualResponse);
            Assert.AreEqual((int)HttpStatusCode.OK, statusCodeResult.StatusCode.GetValueOrDefault());
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
            var registeredProviders = new List<RegisteredProviderModel>();
            
            var apiResponse =
                new ApiResponse<List<RegisteredProviderModel>>(registeredProviders, statusCode,
                    errorMessage);

            mediatorMock.Setup(m => m.Send(It.IsAny<GetRegisteredProvidersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apiResponse);

            var subject = new RegisteredProvidersController(Mock.Of<ILogger<RegisteredProvidersController>>(), mediatorMock.Object);

            var response = await subject.GetRegisteredProviders();

            var statusCodeResult = response as IStatusCodeActionResult;
            
            Assert.AreEqual((int)statusCode, statusCodeResult.StatusCode.GetValueOrDefault());
            Assert.AreEqual(errorMessage, ((ObjectResult)statusCodeResult).Value);
        }
    }
}
