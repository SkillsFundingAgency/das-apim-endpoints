using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Api.Controllers;
using SFA.DAS.Roatp.CourseManagement.Application.Provider;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;

namespace SFA.DAS.Roatp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class ProvidersControllerTests
    {
        const int Validukprn = 101;
        const int Invalidukprn = 999;
        [TestCase(0, 400)]
        [TestCase(-1, 400)]
        [TestCase(Validukprn, 200)]
        [TestCase(Invalidukprn, 404)]
        public async Task GetProvider_ReturnsAppropriateResponse(int Ukprn, int expectedStatusCode)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderQuery>(q => q.Ukprn == Invalidukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderResult(null));
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderQuery>(q => q.Ukprn == Validukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderResult(new GetProviderResponse()));

            var subject = new ProvidersController(Mock.Of<ILogger<ProvidersController>>(), mediatorMock.Object);

            var response = await subject.GetProvider(Ukprn);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual(expectedStatusCode, statusCodeResult.StatusCode.GetValueOrDefault());
        }
    }
}
