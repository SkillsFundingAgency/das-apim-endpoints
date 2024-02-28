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
using SFA.DAS.EmployerIncentives.Application.Commands.SignAgreement;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Account
{
    public class WhenSigningAgreement
    {
        [Test, MoqAutoData]
        public async Task Then_Signs_Agreement_Calling_Mediator(
            long accountId,
            long accountLegalEntityId,
            SignAgreementRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]AccountController controller)
        {
            var controllerResult = await controller.SignAgreement(accountId, accountLegalEntityId, request) as NoContentResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);
            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<SignAgreementCommand>(c=>
                        c.AccountId.Equals(accountId) 
                        && c.AccountLegalEntityId.Equals(accountLegalEntityId)
                        && c.AgreementVersion.Equals(request.AgreementVersion)
                        && c.LegalEntityName.Equals(request.LegalEntityName)
                        && c.LegalEntityId.Equals(request.LegalEntityId)
                        ),
                    It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}