using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.Queries.GetApplicationReviewsCountByAccountId;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerAccounts
{
    [TestFixture]
    public class WhenGettingApplicationReviewsCount
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_From_Mediator(
            long accountId,
            List<long> vacancyReferences,
            string applicationSharedFilteringStatus,
            GetApplicationReviewsCountByAccountIdQueryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApplicationReviewsCountByAccountIdQuery>(c =>
                        c.AccountId.Equals(accountId) && c.VacancyReferences == vacancyReferences &&
                        c.ApplicationSharedFilteringStatus == applicationSharedFilteringStatus),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetApplicationReviewsCount(accountId, applicationSharedFilteringStatus, vacancyReferences) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetApplicationReviewsCountByAccountIdQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            long accountId,
            List<long> vacancyReferences,
            string applicationSharedFilteringStatus,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerAccountsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApplicationReviewsCountByAccountIdQuery>(c =>
                        c.AccountId.Equals(accountId) && c.VacancyReferences == vacancyReferences &&
                        c.ApplicationSharedFilteringStatus == applicationSharedFilteringStatus),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetApplicationReviewsCount(accountId,applicationSharedFilteringStatus, vacancyReferences) as BadRequestResult;

            controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
