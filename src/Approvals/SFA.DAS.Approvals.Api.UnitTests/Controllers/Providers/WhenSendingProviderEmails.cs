using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.ProviderUsers.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Providers
{
    public class WhenSendingProviderEmails
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Provider_Emails(
            ProviderEmailRequest request,
            long ukprn,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<ProviderEmailCommand>(),
                    It.IsAny<CancellationToken>()));

            var controllerResult = await controller.EmailUsers(ukprn, request) as StatusCodeResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            mockMediator.Verify(x=>x.Send(It.Is<ProviderEmailCommand>(p=>p.ProviderId == ukprn), It.IsAny<CancellationToken>()));
        }
    }
}