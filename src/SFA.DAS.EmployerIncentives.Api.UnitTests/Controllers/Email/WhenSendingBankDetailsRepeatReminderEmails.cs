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
    public class WhenSendingBankDetailsRepeatReminderEmails
    {
        [Test, MoqAutoData]
        public async Task Then_Send_Email_Command_Is_Handled(
            BankDetailsRepeatReminderEmailsRequest sendEmailRequest,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmailController controller)
        {
            var controllerResult = await controller.SendBankDetailsRepeatReminderEmails(sendEmailRequest) as OkResult;

            controllerResult.Should().NotBeNull();
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<SendBankDetailsRepeatReminderEmailsCommand>(c =>
                        c.ApplicationCutOffDate.Equals(sendEmailRequest.ApplicationCutOffDate)),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
