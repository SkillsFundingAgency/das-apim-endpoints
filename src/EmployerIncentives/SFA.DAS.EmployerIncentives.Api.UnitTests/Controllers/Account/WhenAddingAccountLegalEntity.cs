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
using SFA.DAS.EmployerIncentives.Application.Commands.AddLegalEntity;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Account
{
    public class WhenAddingAccountLegalEntity
    {
        
        [Test, MoqAutoData]
        public async Task Then_Creates_Account_LegalEntity_From_Mediator(
            long accountId,
            LegalEntityRequest legalEntityRequest,
            CreateAccountLegalEntityCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]AccountController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<CreateAccountLegalEntityCommand>(c=>
                        c.AccountId.Equals(accountId) 
                        && c.OrganisationName.Equals(legalEntityRequest.OrganisationName)
                        && c.LegalEntityId.Equals(legalEntityRequest.LegalEntityId)
                        && c.AccountLegalEntityId.Equals(legalEntityRequest.AccountLegalEntityId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.AddLegalEntity(accountId, legalEntityRequest) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }
    }
}