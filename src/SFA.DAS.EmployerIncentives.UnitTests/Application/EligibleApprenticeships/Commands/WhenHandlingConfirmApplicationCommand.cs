using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.ConfirmApplication;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingConfirmApplicationCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Service_Is_Called_With_The_Request_To_Confirm(
            long accountId,
            Guid applicationId,
            DateTime submitted,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            ConfirmApplicationCommandHandler handler)
        {

            var command = new ConfirmApplicationCommand(applicationId, accountId, submitted);

            await handler.Handle(command, CancellationToken.None);

            employerIncentivesService.Verify(x => x.ConfirmIncentiveApplication(It.Is<ConfirmIncentiveApplicationRequest>(
                r => r.DateSubmitted == command.DateSubmitted &&
                     r.AccountId == command.AccountId &&
                     r.IncentiveApplicationId == command.ApplicationId), CancellationToken.None),
                Times.Once);
        }
    }
}