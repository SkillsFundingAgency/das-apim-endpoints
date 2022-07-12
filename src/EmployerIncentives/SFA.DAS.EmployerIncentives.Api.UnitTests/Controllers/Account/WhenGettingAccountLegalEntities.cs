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
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Account
{
    public class WhenGettingAccountLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Account_LegalEntities_From_Mediator(
            long accountId,
            GetLegalEntitiesResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]AccountController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetLegalEntitiesQuery>(c=>
                        c.AccountId.Equals(accountId) ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetLegalEntities(accountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as AccountLegalEntityDto[];
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult.AccountLegalEntities, options=>options.Excluding(c=>c.LegalEntityName));
        }
    }
}