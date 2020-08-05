using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Command.CreateApplication;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Application
{
    public class WhenCreatingAnApplication
    {
        [Test, MoqAutoData]
        public async Task Then_CreateApplicationCommand_Is_Handled(
            long accountId,
            CreateApplicationRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApplicationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<CreateApplicationCommand>(c =>
                        c.AccountId.Equals(accountId)
                        && c.AccountLegalEntityId.Equals(request.AccountLegalEntityId)
                        && c.ApplicationId.Equals(request.ApplicationId)
                        && c.ApprenticeshipIds.Equals(request.ApprenticeshipIds)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(request.ApplicationId);

            var controllerResult = await controller.PostApplication(request) as CreatedResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
            controllerResult.Location.Should().Be($"/accounts/{request.AccountId}/applications/{request.ApplicationId}");
        }
    }
}