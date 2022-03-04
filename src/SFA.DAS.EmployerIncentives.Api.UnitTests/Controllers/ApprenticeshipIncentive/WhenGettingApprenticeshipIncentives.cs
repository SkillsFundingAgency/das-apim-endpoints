using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApprenticeshipIncentives;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.ApprenticeshipIncentive
{
    public class WhenGettingApprenticeshipIncentives
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeshipIncentives_From_Mediator(
            long accountId,
            long accountLegalEntityId,
            GetApprenticeshipIncentivesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]ApprenticeshipIncentiveController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApprenticeshipIncentivesQuery>(c=>
                        c.AccountId.Equals(accountId) && c.AccountLegalEntityId.Equals(accountLegalEntityId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetApprenticeshipIncentives(accountId, accountLegalEntityId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as ApprenticeshipIncentiveDto[];
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult.ApprenticeshipIncentives);
        }
    }
}