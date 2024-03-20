using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Controllers;
using SFA.DAS.Reservations.Application.ProviderAccounts.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.Api.UnitTests.Controllers.Providers;

public class WhenGettingProviderLegalEntitiesWithCreatCohort
{
    [Test, MoqAutoData]
    public async Task Then_Get_Provider_Legal_Entities_Is_Called(
        int ukprn,
        GetProviderAccountLegalEntitiesWithCreatCohortResponse mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ProviderAccountsController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetProviderAccountLegalEntitiesWithCreatCohortQuery>(c => c.Ukprn.Equals(ukprn)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await controller.GetAccountLegalEntitiesWithCreatCohort(ukprn) as ObjectResult;

        controllerResult!.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetProviderAccountLegalEntitiesWithCreatCohortResponse;
        model!.Should().BeEquivalentTo(mediatorResult);
    }
}