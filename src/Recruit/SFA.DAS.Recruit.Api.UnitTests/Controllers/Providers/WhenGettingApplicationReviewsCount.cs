using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByUkprn;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Providers
{
    [TestFixture]
    public class WhenGettingApplicationReviewsCount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            int ukprn,
            List<long> vacancyReferences,
            GetApplicationReviewsCountByUkprnResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApplicationReviewsCountByUkprnQuery>(c => c.Ukprn.Equals(ukprn) && c.VacancyReferences == vacancyReferences),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetApplicationReviewsCount(ukprn, vacancyReferences) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetApplicationReviewsCountByUkprnResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int ukprn,
            List<long> vacancyReferences,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApplicationReviewsCountByUkprnQuery>(c => c.Ukprn.Equals(ukprn) && c.VacancyReferences == vacancyReferences),
                    It.IsAny<CancellationToken>()))
                 .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetApplicationReviewsCount(ukprn, vacancyReferences) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
