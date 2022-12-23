using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpProviderModeration.Api.Controllers;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Models;
using SFA.DAS.RoatpProviderModeration.Application.Queries.GetProvider;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpProviderModeration.Api.UnitTests.Controllers.ProvidersControllerTests
{
    [TestFixture]
    public class GetProviderTests
    {
        private const int ValidUkprn = 10000001;
        private const string MarketingInfo = "Marketing info";
        private const ProviderType mainProvider = ProviderType.Main;

        [TestCase(ValidUkprn)]
        public async Task GetProviderCourse_ReturnsExpectedState(int ukprn)
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.Is<GetProviderQuery>(q =>  q.Ukprn == ukprn), It.IsAny<CancellationToken>())).ReturnsAsync(new GetProviderQueryResult { MarketingInfo = MarketingInfo, ProviderType = mainProvider });

            var controller = new ProvidersController(Mock.Of<ILogger<ProvidersController>>(), mediatorMock.Object);

            var response = await controller.GetProvider(ukprn);

            var statusCodeResult = response as IStatusCodeActionResult;

            Assert.AreEqual(200, statusCodeResult?.StatusCode.GetValueOrDefault());
        }

        [Test]
        public async Task GetProviderCourse_Null_ResultReturnsNotFound()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(m => m.Send(It.IsAny<GetProviderQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((GetProviderQueryResult)null);
        
            var controller = new ProvidersController(Mock.Of<ILogger<ProvidersController>>(), mediatorMock.Object);
        
            var response = await controller.GetProvider(ValidUkprn);
        
            var statusCodeResult = response as IStatusCodeActionResult;
        
            Assert.AreEqual(404, statusCodeResult?.StatusCode.GetValueOrDefault());
        }
    }
}
