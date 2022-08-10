using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetProvider;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers.ProvidersControllerTests
{
    [TestFixture]
    public class GetProviderTests
    {
        private const int ValidUkprn = 10000001;
        private const string MarketingInfo = "Marketing info";

        [TestCase(ValidUkprn)]
        public async Task GetProviderCourse_ReturnsExpectedState(int ukprn)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderQuery>(q =>  q.Ukprn == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderResult { MarketingInfo = MarketingInfo});

            var controller = new ProvidersController(Mock.Of<ILogger<ProvidersController>>(), mediatorMock.Object);

            var response = await controller.GetProvider(ukprn);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual(200, statusCodeResult.StatusCode.GetValueOrDefault());
        }

        [Test]
        public async Task GetProviderCourse_Null_ResultReturnsNotFound()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((GetProviderResult)null);
        
            var controller = new ProvidersController(Mock.Of<ILogger<ProvidersController>>(), mediatorMock.Object);
        
            var response = await controller.GetProvider(ValidUkprn);
        
            var statusCodeResult = response as IStatusCodeActionResult;
        
            Assert.AreEqual(404, statusCodeResult.StatusCode.GetValueOrDefault());
        }
    }
}
