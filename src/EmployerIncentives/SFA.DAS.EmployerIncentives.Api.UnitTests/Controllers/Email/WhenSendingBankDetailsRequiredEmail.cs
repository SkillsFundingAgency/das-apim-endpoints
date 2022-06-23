using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.SendEmail;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Email
{    
    public class WhenSendingBankDetailsRequiredEmail
    {
        [Test, MoqAutoData]
        public async Task Then_Send_Email_Command_Is_Handled(
            SendBankDetailsEmailRequest sendEmailRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmailController controller)
        {
            var controllerResult = await controller.SendBankDetailsRequiredEmail(sendEmailRequest) as OkResult;

            controllerResult.Should().NotBeNull();
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<SendBankDetailsRequiredEmailCommand>(c =>
                        c.AccountId.Equals(sendEmailRequest.AccountId)
                        && c.AccountLegalEntityId.Equals(sendEmailRequest.AccountLegalEntityId)
                        && c.EmailAddress.Equals(sendEmailRequest.EmailAddress)
                        && c.AddBankDetailsUrl.Equals(sendEmailRequest.AddBankDetailsUrl)),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
