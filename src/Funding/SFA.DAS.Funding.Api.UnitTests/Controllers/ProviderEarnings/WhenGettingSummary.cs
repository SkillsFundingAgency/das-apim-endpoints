using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Api.Controllers;
using SFA.DAS.Funding.Api.Models;
using SFA.DAS.Funding.Application.Queries.GetProviderEarningsSummary;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.Api.UnitTests.Controllers.ProviderEarnings
{
    public class WhenGettingSummary
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Application_From_Mediator(
            long ukprn,
            GetProviderEarningsSummaryResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProviderEarningsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetProviderEarningsSummaryQuery>(c=> c.Ukprn.Equals(ukprn)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetSummary(ukprn) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetProviderEarningsSummaryResponse;
            Assert.That(model, Is.Not.Null);
            model.Summary.Should().BeEquivalentTo(mediatorResult.Summary);
        }
    }
}