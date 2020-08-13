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
using SFA.DAS.EmployerIncentives.Application.Commands.RemoveLegalEntity;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Account
{
    public class WhenRemovingAccountLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_Removes_Account_LegalEntity_Calling_Mediator(
            long accountId,
            long accountLegalEntityId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]AccountController controller)
        {
            var controllerResult = await controller.RemoveLegalEntity(accountId, accountLegalEntityId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<RemoveAccountLegalEntityCommand>(c=>
                        c.AccountId.Equals(accountId) 
                        && c.AccountLegalEntityId.Equals(accountLegalEntityId)),
                    It.IsAny<CancellationToken>()), Times.Once);

        }
    }
}